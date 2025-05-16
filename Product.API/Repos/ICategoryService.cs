using Product.Model;

namespace Product.API.Repos
{
    public interface ICategoryService
    {
        Task<(CategoryModel, string)> InsertCategory(CategoryModel data);
        Task<string> DeleteCategory(int Id);
        Task<(List<CategoryModel>, string)> GetAllCategoryList();
    }
}
