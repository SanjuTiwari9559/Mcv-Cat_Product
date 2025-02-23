using Microsoft.EntityFrameworkCore;
using Task_Cat_ProMvc.Models.Data;
using Task_Cat_ProMvc.Services;

namespace Task_Cat_ProMvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<Cat_ProductDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Product_Category")));

            // Read the configuration value
            var useApi = builder.Configuration.GetValue<bool>("UseApi");

            if (useApi)
            {
                builder.Services.AddScoped<ICategory, CategoryApi>();
            }
            else
            {
                builder.Services.AddScoped<ICategory, CategoryMvc>();
            }

            // Register HttpClient for API calls
            builder.Services.AddHttpClient();

            var app = builder.Build();

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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

