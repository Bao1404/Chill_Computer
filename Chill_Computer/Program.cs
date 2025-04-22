using Chill_Computer.Models;
using Chill_Computer.Contacts;
using Chill_Computer.Services;
using Microsoft.EntityFrameworkCore;

namespace Chill_Computer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ChillComputerContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Chill_Computer") ?? throw new InvalidOperationException("Connection string 'Chill_Computer' not found"))
            );

            builder.Services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
            builder.Services.AddScoped<IProductTypeFilterRepository, ProductTypeFilterRepository>();
            builder.Services.AddScoped<IFilterCategoryRepository, FilterCategoryRepository>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IAttributeRepository, AttributeRepository>();  
            builder.Services.AddScoped<IProductAttributeRepository, ProductAttributeRepository>();
            builder.Services.AddScoped<IBrandRepository, BrandRepository>();
            builder.Services.AddScoped<IProfileService, ProfileService>();
            builder.Services.AddScoped<IOrderHistoryService, OrderHistoryService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();

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
