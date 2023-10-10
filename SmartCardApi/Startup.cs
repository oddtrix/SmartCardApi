using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartCardApi.Models.AutoMapperConfig;
using SmartCardApi.Models.Cards;
using SmartCardApi.Models.Identity;
using SmartCardService.Models;
using SmartCardService.Services;
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
            services.AddSwaggerGen();

            // Identity conf
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(
                opts => opts.SignIn.RequireConfirmedEmail = true);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
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

            //AutoMapper
            services.AddAutoMapper(typeof(AutoMapperConfigProfile));

            //Database
            string connectionString = this.Configuration["ConnectionString:DefaultConnection"];

            services.AddDbContext<CardDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            //Email Config
            var emailConfig = Configuration.GetSection("EmailConfiguration")
                                           .Get<EmailConfiguration>();

            // DI
            services.AddScoped<ICardRepository, CardRepository>();
            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailService, EmailService>();
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(options =>
            {
                options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });

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
