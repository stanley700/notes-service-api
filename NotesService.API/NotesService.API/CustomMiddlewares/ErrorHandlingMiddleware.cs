using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NotesService.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NotesService.API.CustomMiddlewares
{
    public class ErrorHandlingMiddleware 
    {
        private RequestDelegate _next;
        ILogger<ErrorHandlingMiddleware> _logger;
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                //Handle exception
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            if(exception is MessageException)
            {
                code = ((MessageException)exception).HttpStatusCode;
            }else if(exception is UnauthorizedAccessException)
            {
                code = HttpStatusCode.Unauthorized;
            }

            return httpContext.Response.WriteAsJsonAsync(new { responseCode = code, message = exception.Message });
        }
    }
}
