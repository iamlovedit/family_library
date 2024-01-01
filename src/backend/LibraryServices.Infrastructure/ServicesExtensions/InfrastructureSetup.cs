﻿using AutoMapper;
using LibraryServices.Infrastructure.Consul;
using LibraryServices.Infrastructure.Filters;
using LibraryServices.Infrastructure.Repository;
using LibraryServices.Infrastructure.Sercurity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using System.IdentityModel.Tokens.Jwt;

namespace LibraryServices.Infrastructure.ServicesExtensions
{
    public static class InfrastructureSetup
    {
        public static void AddInfrastructureSetup(this WebApplicationBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);
            var services = builder.Services;
            var configuration = builder.Configuration;

            services.AddHealthChecks();
            services.AddConsulSetup(configuration);
            services.AddSingleton<SecurityTokenValidator>();
            services.AddSingleton<JwtSecurityTokenHandler>();
            services.AddSingleton<GalaTokenHandler>();
            services.AddSingleton<IAESEncryptionService, AESEncryptionService>();
            services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, JwtBearerOptionsPostConfigureOptions>();
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<ITokenBuilder, TokenBuilder>();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.None,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };

            services.AddSingleton(provider => new MapperConfiguration(config =>
            {
                config.AddProfile(new MappingProfile());
            }).CreateMapper());

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("cors", policy =>
                {
                    policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
                });
            });

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(GlobalExceptionsFilter));
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });

            services.AddDatabaseSeedSetup();

            services.AddSqlsugarSetup(configuration, builder.Environment);

            services.AddRedisCacheSetup(configuration);

            services.AddApiVersionSetup();

            services.AddJwtAuthenticationSetup(configuration);

            services.AddAuthorizationSetup(configuration);

            builder.AddSerilogSetup(configuration);

            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
            });
        }
    }
}
