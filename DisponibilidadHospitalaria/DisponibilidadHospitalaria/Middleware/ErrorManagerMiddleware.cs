using Aplicacion;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace DisponibilidadHospitalaria.Middleware
{
    public class ErrorManagerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorManagerMiddleware> _logger;

        public ErrorManagerMiddleware(RequestDelegate next, ILogger<ErrorManagerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await ExceptionManagerAsync(context, ex);
            }
        }

        private object errores;

        private async Task ExceptionManagerAsync(HttpContext context, Exception ex)
        {
            switch (ex)
            {
                case AppException ex1:
                    _logger.LogError(ex, "Manejador de error");
                    errores = ex1.Errores;
                    context.Response.StatusCode = (int)ex1.Code;
                    break;
                case Exception ex2:
                    _logger.LogError(ex, "Error del servidor");
                    errores = string.IsNullOrWhiteSpace(ex2.Message) ? "Error" : ex2.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.ContentType = "application/json";

            if (errores != null)
            {
                var resultados = JsonSerializer.Serialize(new { errores });
                await context.Response.WriteAsync(resultados);
            }
        }
    }
}
