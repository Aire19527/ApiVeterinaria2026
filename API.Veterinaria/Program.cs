using API.Veterinaria.Handlers;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using Veterina.Core.Data;
using Veterinaria.Entity.DTO;

var builder = WebApplication.CreateBuilder(args);


#region SQL SERVER
// Fuera de Docker usa appsettings.json. En el contenedor, ConnectionStrings__ConnectionStringSQLServer
// del compose pisa esa cadena.
builder
    .Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStringSQLServer"));
});
#endregion

#region Inyection
DependencyInyectionHandler.DependencyInyectionConfig(builder.Services);
#endregion


#region JWT
// Tokens:Key, Issuer, Audience y ExpireMinutes. En Docker los pisan Tokens__Key, Tokens__Issuer,
// Tokens__Audience y Tokens__ExpireMinutes, con el mismo valor en todas las réplicas.
var tokenAppSetting = builder.Configuration.GetSection("Tokens");
JwtConfigurationHandler.ConfigureJwtAuthentication(builder.Services, tokenAppSetting);
#endregion

// Add services to the container.

// nginx reenvía la IP y el protocolo originales. Sin vaciar las listas, ASP.NET ignora esos
// headers porque el proxy de Docker no está entre las redes que confía por defecto.
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

// El healthcheck del compose llama a /health. Revisa que el proceso responda y que la base abra.
builder.Services.AddHealthChecks()
    .AddDbContextCheck<DataContext>();

builder.Services.AddControllers(options =>
{
    // Aplica a todos los controladores. Ya no hace falta [TypeFilter(typeof(CustomExceptionHandler))].
    options.Filters.Add<CustomExceptionHandler>();
})
.ConfigureApiBehaviorOptions(options =>
{
    // Los [Required] y [MaxLength] fallan antes de entrar a la acción.
    // Se responden con ResponseDto en lugar del ProblemDetails por defecto.
    options.InvalidModelStateResponseFactory = context =>
    {
        Dictionary<string, string[]> errors = context.ModelState
            .Where(entry => entry.Value?.Errors.Count > 0)
            .ToDictionary(
                entry => entry.Key,
                entry => entry.Value!.Errors.Select(error => error.ErrorMessage).ToArray());

        return new BadRequestObjectResult(new ResponseDto
        {
            IsSuccess = false,
            Message = string.Join(" ", errors.SelectMany(error => error.Value)),
            Result = errors
        });
    };
});

// El documento OpenAPI sale en /openapi/v1.json. Scalar es la interfaz que lo muestra.
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new OpenApiInfo
        {
            Title = "Veterinaria",
            Version = "v1",
            Description = "Api veterinaria",
            Contact = new OpenApiContact
            {
                Name = "Veterinaria",
                Email = "admin@veterinaria.com",
                Url = new Uri("https://www.lab.com"),
            }
        };

        // Esquema HTTP Bearer: en Scalar se pega solo el token y la interfaz agrega el prefijo.
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Pega solo el token JWT. Scalar agrega el prefijo Bearer."
        };

        document.Security ??= [];
        document.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = []
        });

        return Task.CompletedTask;
    });
});


var app = builder.Build();

// Solo el servicio migrator trae estas variables. Aplica las migraciones y sale sin abrir el puerto.
// Las réplicas no las tienen, así que no compiten por migrar.
if (app.Configuration.GetValue<bool>("APPLY_MIGRATIONS"))
{
    using var scope = app.Services.CreateScope();
    DataContext db = scope.ServiceProvider.GetRequiredService<DataContext>();
    db.Database.Migrate();
    if (app.Configuration.GetValue<bool>("MIGRATE_ONLY"))
        return;
}

// Configure the HTTP request pipeline.
// En Docker el entorno es Development, así que Scalar queda en http://localhost:8080/scalar/v1.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// En el contenedor ASPNETCORE_URLS es http://+:8080. Sin "https" en la URL no hay redirección,
// porque dentro de Docker solo hay HTTP y el redirect rompería las llamadas del proxy.
string urls = app.Configuration["ASPNETCORE_URLS"] ?? "";
if (app.Environment.IsDevelopment() && urls.Contains("https", StringComparison.OrdinalIgnoreCase))
    app.UseHttpsRedirection();

app.UseForwardedHeaders();

// INSTANCE_PREFIX viene del compose. Junto con el número del hostname identifica la réplica.
string instanceName = InstanceName(app.Configuration["INSTANCE_PREFIX"]);

// Header en todas las respuestas, para ver qué réplica atendió sin llamar a /instance.
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Instance"] = instanceName;
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

// Sin autenticación: el healthcheck de Docker no envía token.
app.MapHealthChecks("/health").AllowAnonymous();
// GET /veterinaria/instance a través del proxy. Repetirlo alterna entre las réplicas.
app.MapGet("/instance", () => Results.Ok(new { instance = instanceName }))
    .AllowAnonymous();

app.MapControllers();

app.Run();

// Docker nombra el hostname appveterinaria-1, appveterinaria-2, ...
// Con INSTANCE_PREFIX=AppVeterinaria la respuesta queda AppVeterinaria-1.
// Sin ese prefijo (Visual Studio) se devuelve el nombre del PC.
static string InstanceName(string? prefix)
{
    string host = Environment.MachineName;
    if (string.IsNullOrWhiteSpace(prefix))
        return host;

    int dash = host.LastIndexOf('-');
    if (dash > 0 && int.TryParse(host.AsSpan(dash + 1), out _))
        return prefix + host[dash..];

    return host;
}
