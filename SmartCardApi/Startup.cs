﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmartCardApi.BLL.Services;
using SmartCardApi.BusinessLayer.Interfaces;
using SmartCardApi.BusinessLayer.Services;
using SmartCardApi.Contexts;
using SmartCardApi.Models.AutoMapperConfig;
using SmartCardApi.Models.Cards;
using SmartCardApi.Models.Identity;
using SmartCardApi.Models.User;
using System.Text;

namespace SmartCardApi
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //Database
            string connectionString = this.Configuration["ConnectionString:DefaultConnection"];
            string identityString = this.Configuration["ConnectionString:IdentityConnection"];

            services.AddDbContext<AppDomainDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(identityString);
            });

            // Identity conf
            services.AddIdentity<AppIdentityUser, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // WTF WHY THIS FIXED ALL THIS SHIT 
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    }; 
                });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            //AutoMapper
            services.AddAutoMapper(typeof(AutoMapperConfigProfile));

            // DI
            services.AddScoped<ICardRepository, CardRepository>();
            services.AddScoped<IAppDomainRepository, AppDomainRepository>();
            services.AddScoped<ICardService, CardService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAdminService, AdminService>();
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors();

            app.UseStatusCodePages();
            app.UseStaticFiles();  
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(options =>
            {
                options.MapControllers();
            });
        }
    }
}
