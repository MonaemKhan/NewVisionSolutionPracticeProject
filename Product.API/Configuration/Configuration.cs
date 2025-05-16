using Microsoft.EntityFrameworkCore;
using Product.API.DBConnection;
using Product.API.Repos;

namespace Product.API.Configuration
{
    public static class Configuration
    {
        public static IServiceCollection ConfigurationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDBContecxt>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConection")));

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            return services;
        }
    }
}
