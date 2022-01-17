using Microsoft.EntityFrameworkCore;
using Shopping.Infrastructure;

namespace Shopping
{
    public  static class Startup
    {
        public static WebApplication InitializeApp(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);
            var app = builder.Build();
            Configure(app);
            return app;

        }
        
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews();
            var connectionString = builder.Configuration.GetConnectionString("ShoppingContext");

            builder.Services.AddDbContext<ShoppingContext>(options =>
                            options.UseSqlServer(connectionString));


        }
        public static void Configure(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                /*
                 * endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
                */

                endpoints.MapControllerRoute(
                    name: "Admin",
                    //areaName: "Admin",
                    //pattern: "{area}/{controller=Home}/{action=Index}/{id?}"
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                    );
            });
        }
    }
}
