using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PsychoCare.API.Confiugration;
using PsychoCare.API.Filters;
using PsychoCare.API.Models;
using PsychoCare.Common.Constants;
using PsychoCare.Common.Interfaces;
using PsychoCare.DataAccess;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace PsychoCare.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PsychoCare API v1");
                c.RoutePrefix = string.Empty;
            });
            app.UseCors("AllowAll");
            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSimpleInjector(ContainerHolder.Container);

            ConfigureApplication();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore); ;

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo() { Title = "PsychoCare API", Version = "v1" });
            });

            services.AddCors(options => options.AddPolicy("AllowAll", p => p
                   .AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader()));

            services.UseSimpleInjectorAspNetRequestScoping(ContainerHolder.Container);
            services.AddSimpleInjector(ContainerHolder.Container, options =>
            {
                options.Container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
                options.AddAspNetCore().AddControllerActivation();
            });

            ConfigureOAuth2(services);
        }

        private static void ConfigureOAuth2(IServiceCollection services)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(Constants.SECRET_KEY);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                    ValidateIssuer = true,
                    ValidAudience = Constants.AUDIENCE_KEY,
                    ValidIssuer = Constants.AUDIENCE_KEY,
                    ValidateAudience = true
                };
            });

            services.AddMvc(config =>
            {
                AuthorizationPolicy policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                config.Filters.Add(new AuthorizeFilter(policy));
                config.Filters.Add(new ErrorFilter());
            });
        }

        private void ConfigureApplication()
        {
            PsychoCareContextInitializer.Initialize();
            ContainerHolder.RegisterCommonDependencies();
            ContainerHolder.Container.Register<IWebPrincipal, WebPrincipal>(Lifestyle.Scoped);
            ContainerHolder.Container.Verify();
        }
    }
}