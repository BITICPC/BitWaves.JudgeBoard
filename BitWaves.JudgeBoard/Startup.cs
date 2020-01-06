using System;
using AutoMapper;
using BitWaves.JudgeBoard.Models;
using BitWaves.JudgeBoard.Services;
using BitWaves.JudgeBoard.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BitWaves.JudgeBoard
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private IConfigurationSection GetJudgeConfig()
        {
            var section = Configuration.GetSection("Judge").GetSection("Authentication");
            if (!section.Exists())
            {
                throw new InvalidOperationException("Configuration section not found: Judge.Authentication");
            }

            return section;
        }

        private SecurityKey GetJwtIssuerSigningKey()
        {
            var configSection = GetJudgeConfig();
            var keyFileName = configSection.GetValue<string>("JwtSigningKey");
            var key = Pem.ReadRsaKey(keyFileName);
            return new RsaSecurityKey(key);
        }

        private void ConfigureJwtServices(IServiceCollection services)
        {
            var signingKey = GetJwtIssuerSigningKey();
            services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.SaveToken = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                    });
            services.AddDefaultJwtIssuer(options =>
            {
                var configSection = GetJudgeConfig();
                options.Expiration = TimeSpan.FromMinutes(configSection.GetValue<int>("JwtExpiration"));
                options.SigningKey = signingKey;
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            ConfigureJwtServices(services);

            services.AddLocalJudgeNodeManager();
            services.AddLocalJudgeAuthenticationService(options =>
            {
                var configSection = GetJudgeConfig();
                options.Expiration = TimeSpan.FromMinutes(configSection.GetValue<int>("SessionExpiresIn"));
                options.LoadJudgePublicKeyFromCertificate(configSection.GetValue<string>("ChallengePublicKey"));
            });
            services.AddAutoMapper(typeof(ModelMappingProfile));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(builder =>
            {
                builder.AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowAnyOrigin();
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
