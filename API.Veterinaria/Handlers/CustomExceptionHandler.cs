using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Veterinaria.Common.Exceptions;
using Veterinaria.Common.Resources;
using Veterinaria.Entity.DTO;

namespace API.Veterinaria.Handlers
{
    public class CustomExceptionHandler : ExceptionFilterAttribute
    {
        /// <summary>
        /// Metodo encargado de capturar todas las Excepciones del proyecto.
        /// Se registra una sola vez como filtro global en Program.cs (options.Filters.Add),
        /// por lo que aplica a todos los controladores sin decorarlos.
        /// Solo cubre excepciones dentro de MVC; el 401 y 403 se formatean en JwtConfigurationHandler.
        /// </summary>
        /// <param name="exception"> Parametro donde queda capturada la Exception </param>
        public override void OnException(ExceptionContext context)
        {
            HttpResponseException responseException = new HttpResponseException();

            ResponseDto response = new ResponseDto();
            if (context.Exception is BusinessException)
            {
                responseException.Status = StatusCodes.Status400BadRequest;
                response.Message = context.Exception.Message;
                context.ExceptionHandled = true;
                //Log.Error(context.Exception, response.Message);
            }
            else
            {
                responseException.Status = StatusCodes.Status500InternalServerError;
                response.Result = new
                {
                    Type = context.Exception.GetType().Name,
                    Message = context.Exception.Message
                };
                response.Message = GeneralMessages.Error500;
                context.ExceptionHandled = true;

                //Add Logs
                //Log.Fatal(context.Exception, GeneralMessages.Error500);

            }

            context.Result = new ObjectResult(responseException.Value)
            {
                StatusCode = responseException.Status,
                Value = response
            };
        }
    }
}
