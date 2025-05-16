
using Product.Model;

namespace Product.API.Repos
{
    public interface IProductService
    {
        Task<(ProductModel, string)> InsertUpdateProduct(ProductModel _product);
        Task<string> DeleteProduct(int Id);
        Task<(List<ProductModel>, string)> GetAllProductList();
    }
}
