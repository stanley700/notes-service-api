using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NotesService.Core.Data;
using NotesService.Core.Services;
using NotesService.Core.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotesService.Core.Data.Entities;
using NotesService.Core.Resources;
using NotesService.API.CustomMiddlewares;
using Microsoft.AspNetCore.Authorization;
using NotesService.API.CustomHandlers;

namespace NotesService.API
{
    public class Startup
    {
        SymmetricSecurityKey SigningKey { get; set; }
        static string SecretKey { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            SecretKey = Configuration[Constants.TokenProviderBearerKeyPlaceHolder];
            SigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();            
            services.AddDbContext<NotesServiceDbContext>(opt => opt.UseInMemoryDatabase("note-service-api-db"));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<INoteService, NoteService>();
            services.AddSingleton(Configuration);

            services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();
            
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = SigningKey,

                ValidateIssuer = true,
                ValidIssuer = Configuration[Constants.TokenProviderIssuerPlaceHolder],

                ValidateAudience = true,
                ValidAudience = Configuration[Constants.TokenProviderAudiencePlaceHolder],

                ValidateLifetime = true
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Notes Service.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, NotesServiceDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notes Service.API v1"));
            }


            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseMiddleware<TokenProviderMiddleware>();
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            AddDemoData(dbContext);
        }

        private static void AddDemoData(NotesServiceDbContext dbContext)
        {
            var adminRole = new Role
            {
                Id = 1,
                Name = Constants.Role_Admin
            };
            dbContext.Roles.Add(adminRole);

            var userRole = new Role
            {
                Id = 2,
                Name = Constants.Role_User
            };
            dbContext.Roles.Add(userRole);

            var user1 = new User
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Stanley",
                LastName = "Okpala",
                Username = "stanley700@outlook.com",
                Password = CryptoService.GenerateHash("stanley"),
                IsActive = true,
                RoleId = adminRole.Id
            };

            dbContext.Users.Add(user1);

            dbContext.SaveChanges();
        }

    }
}
