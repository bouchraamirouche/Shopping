using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shopping.Infrastructure;
using Shopping.Models;

namespace Shopping
{
    public static class Startup
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
            builder.Services.AddMemoryCache();
            builder.Services.AddSession();

            builder.Services.AddRouting(
                Options =>Options.LowercaseUrls = true);


            builder.Services.AddControllersWithViews();
            var connectionString = builder.Configuration.GetConnectionString("ShoppingContext");

            builder.Services.AddDbContext<ShoppingContext>(options =>
                            options.UseSqlServer(connectionString));



            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            })
                      .AddEntityFrameworkStores<ShoppingContext>()
                .AddDefaultTokenProviders();
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
            app.UseSession();


            app.UseAuthentication();
            app.UseAuthorization();





            /*
            endpoints.MapControllerRoute(
             "pages",
             "{slug?}",
             defaults: new { controller ="Pages",action="Page"}
                );



            endpoints.MapControllerRoute(
            "products",
            "products/{categorySlug}",
            defaults: new { controller = "Products", action = "ProductsByCategory" }
               );


            endpoints.MapControllerRoute(
                name: "Admin",
                //areaName: "Admin",
                //pattern: "{area}/{controller=Home}/{action=Index}/{id?}"
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );


            endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

          */
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "pages",
                    "{slug?}",
                    defaults: new { controller = "Pages", action = "Page" }
                );

                endpoints.MapControllerRoute(
                    "products",
                    "products/{categorySlug}",
                    defaults: new { controller = "Products", action = "ProductsByCategory" }
                );

                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });

        }
    }
}

