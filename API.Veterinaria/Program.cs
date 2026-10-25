using API.Veterinaria.Handlers;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using Veterina.Core.Data;

var builder = WebApplication.CreateBuilder(args);


#region SQL SERVER
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
var tokenAppSetting = builder.Configuration.GetSection("Tokens");
JwtConfigurationHandler.ConfigureJwtAuthentication(builder.Services, tokenAppSetting);
#endregion

// Add services to the container.

builder.Services.AddControllers(options =>
{
    // Aplica a todos los controladores. Ya no hace falta [TypeFilter(typeof(CustomExceptionHandler))].
    options.Filters.Add<CustomExceptionHandler>();
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
