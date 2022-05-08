using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NotesService.API.Models;
using NotesService.Core;
using NotesService.Core.Data.Entities;
using NotesService.Core.Resources;
using NotesService.Core.Services;
using NotesService.Core.Services.IServices;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NotesService.API.CustomMiddlewares
{
    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public TokenProviderMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public Task Invoke(HttpContext httpContext, IUserService userService)
        {
            if(!httpContext.Request.Path.Equals(_configuration[Constants.TokenProviderPathPlaceHolder], StringComparison.Ordinal))
            {
                return _next(httpContext);
            }

            if(!httpContext.Request.Method.Equals("POST"))
            {
                httpContext.Response.StatusCode = 400;
                return httpContext.Response.WriteAsync("Bad request");
            }

            return GenerateToken(httpContext, userService);
        }

        private async Task GenerateToken(HttpContext httpContext, IUserService userService)
        {
            httpContext.Response.ContentType = "application/json";
            var initialBody = httpContext.Request.Body;
            try
            {
                httpContext.Request.EnableBuffering();
                var body = string.Empty;
                using (StreamReader reader = new StreamReader(httpContext.Request.Body))
                {
                    body = await reader.ReadToEndAsync();
                }

                if (string.IsNullOrEmpty(body))
                {
                    httpContext.Response.StatusCode = 400;
                    await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new
                    {
                        invalid_grant = SystemCodes.DataSecurityViolation,
                        ResponseDescription = "Invalid username/password",
                        ResponseCode = SystemCodes.DataSecurityViolation
                    }));
                    return;
                }

                var username = JsonConvert.DeserializeObject<AuthenticationRequest>(body)?.Username?.Trim();
                var secret = httpContext.Request.Headers["secret"].ToString();

                if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(secret))
                {
                    httpContext.Response.StatusCode = 400;
                    await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new
                    {
                        invalid_grant = SystemCodes.DataSecurityViolation,
                        ResponseDescription = "Invalid username/password",
                        ResponseCode = SystemCodes.DataSecurityViolation
                    }));
                    return;
                }

                var user = await userService.GetUser(username, CryptoService.GenerateHash(secret));
                if(user == null)
                {
                    httpContext.Response.StatusCode = 400;
                    await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new
                    {
                        invalid_grant = SystemCodes.DataSecurityViolation,
                        ResponseDescription = "Invalid username/password",
                        ResponseCode = SystemCodes.DataSecurityViolation
                    }));
                    return;
                }

                var accessDetails = GenerateAccessToken(user);

                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(accessDetails, new JsonSerializerSettings { Formatting = Formatting.Indented }));
            }
            catch(Exception ex)
            {
                
            }
            finally
            {

                httpContext.Response.Body = initialBody;
            }

        }

        private object GenerateAccessToken(User user)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Username),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
            };

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration[Constants.TokenProviderBearerKeyPlaceHolder])), SecurityAlgorithms.HmacSha256);
            var expiresIn = DateTime.UtcNow.AddMinutes(720);
            var jwt = new JwtSecurityToken(
                issuer: _configuration[Constants.TokenProviderIssuerPlaceHolder],
                audience: _configuration[Constants.TokenProviderAudiencePlaceHolder],
                claims: claims,
                expires: expiresIn,
                signingCredentials: signingCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                token = encodedJwt,
                expires_in = expiresIn,
                token_type = "bearer",
                scope = "scope",
                firstname = user.FirstName,
                lastname = user.LastName,
                jti = user.Id,
                username = user.Username
            };

            return response;
        }
    }
}
