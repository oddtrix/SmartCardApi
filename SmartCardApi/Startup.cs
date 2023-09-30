using Microsoft.EntityFrameworkCore;
using SmartCardApi.Models;

namespace SmartCardApi
{
    public class Startup
    {
        private IConfiguration Configuration { get; set; }

        public Startup(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen();


            string connString = Configuration["ConnectionString:DefaultConnection"];
            services.AddDbContext<CarDDbContext>(options =>
            {
                options.UseSqlServer(connString);
            });

            services.AddTransient<ICardRepository, CardRepository>();
            services.AddMvc(opts => opts.EnableEndpointRouting = false);
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();  
            app.UseHttpsRedirection();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}");
            });
        }
    }
}
