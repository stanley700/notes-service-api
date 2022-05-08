using Microsoft.AspNetCore.Http;
using NotesService.Core.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesService.API.CustomMiddlewares
{
    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate _next;
        public TokenProviderMiddleware(RequestDelegate next, IUserService userService)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {


            return GenerateToken(httpContext);
        }

        private async Task GenerateToken(HttpContext httpContext)
        {

        }
    }
}
