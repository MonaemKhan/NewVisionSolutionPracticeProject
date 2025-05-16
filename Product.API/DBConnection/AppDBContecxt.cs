using Microsoft.EntityFrameworkCore;
using Product.Model;

namespace Product.API.DBConnection
{
    public class AppDBContecxt : DbContext
    {
        public AppDBContecxt(DbContextOptions<AppDBContecxt> options) : base(options) { }

        public DbSet<ProductModel> Products { get; set; }
        public DbSet<CategoryModel> Category { get; set; }
    }
}
