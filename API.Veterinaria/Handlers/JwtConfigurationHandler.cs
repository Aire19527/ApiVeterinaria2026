using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Veterinaria.Common.Resources;
using Veterinaria.Entity.DTO;

namespace API.Veterinaria.Handlers
{
    public static class JwtConfigurationHandler
    {
        public static void ConfigureJwtAuthentication(IServiceCollection services, IConfigurationSection tokenAppSetting)
        {
            string? issuer = tokenAppSetting.GetSection("Issuer").Value;
            string? audience = tokenAppSetting.GetSection("Audience").Value;
            string? key = tokenAppSetting.GetSection("Key").Value;

            if (string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience) || string.IsNullOrWhiteSpace(key))
                throw new InvalidOperationException("La configuración Tokens debe incluir Key, Issuer y Audience.");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddCookie()
               .AddJwtBearer(cfg =>
               {
                   cfg.TokenValidationParameters = new TokenValidationParameters
                   {
                       // Rechaza el token si no lo emitió esta API (debe coincidir con Tokens:Issuer).
                       ValidateIssuer = true,
                       // Rechaza el token si no va dirigido a esta API (debe coincidir con Tokens:Audience).
                       ValidateAudience = true,
                       // Rechaza el token cuando ya pasó la fecha exp.
                       ValidateLifetime = true,
                       // Comprueba que la firma se haya hecho con nuestra clave secreta.
                       ValidateIssuerSigningKey = true,
                       // Por defecto se suman 5 minutos de tolerancia. En cero, el token caduca justo al llegar exp.
                       ClockSkew = TimeSpan.Zero,
                       // Emisor esperado. Tiene que ser el mismo que se escribe al generar el token.
                       ValidIssuer = issuer,
                       // Audiencia esperada. Tiene que ser la misma que se escribe al generar el token.
                       ValidAudience = audience,
                       // Clave con la que se verifica la firma. Tiene que ser la misma Tokens:Key usada al firmar.
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                   };

                   // El 401 y el 403 los emite este middleware antes de llegar al controlador,
                   // así que CustomExceptionHandler no los ve. Aquí se les da el formato ResponseDto.
                   cfg.Events = new JwtBearerEvents
                   {
                       // 401: no hay token, está mal firmado o ya venció.
                       OnChallenge = async context =>
                       {
                           // Evita la respuesta vacía por defecto.
                           context.HandleResponse();
                           await WriteResponse(context.Response, StatusCodes.Status401Unauthorized, GeneralMessages.Error401);
                       },
                       // 403: el token es válido pero el usuario no tiene permiso.
                       OnForbidden = context =>
                           WriteResponse(context.Response, StatusCodes.Status403Forbidden, GeneralMessages.Error403)
                   };
               });
        }

        private static Task WriteResponse(HttpResponse response, int statusCode, string message)
        {
            response.StatusCode = statusCode;
            response.ContentType = "application/json";
            ResponseDto body = new ResponseDto { IsSuccess = false, Message = message, Result = null };
            return response.WriteAsJsonAsync(body);
        }
    }
}
