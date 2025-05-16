using Product.API.DBConnection;
using Product.Model;
using System.Data;

namespace Product.API.Repos
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDBContecxt _dbContext;

        public CategoryService(AppDBContecxt dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(CategoryModel,string)> InsertCategory(CategoryModel data)
        {
            DataAccessManager<AppDBContecxt> con = new DataAccessManager<AppDBContecxt>();
            try
            {
                await con.GetDataAccess(_dbContext, true);
                con.AddParam("name", data.Name, SqlDbType.VarChar);
                con.AddOutputParam("errMsg", SqlDbType.VarChar);
                await con.executeNonQueryAsync("InsertCategoty");
                await con.commitAsync();
                return (data,con.outPutParamVaLue["errMsg"].ToString());
            }
            catch (Exception ex)
            {
                await con.rollbackAsync();
                return (new CategoryModel(),ex.Message.ToString());
            }
            finally
            {
                await con.disposeAsync();
            }
        }
        public async Task<string> DeleteCategory(int Id)
        {
            DataAccessManager<AppDBContecxt> con = new DataAccessManager<AppDBContecxt>();
            try
            {
                await con.GetDataAccess(_dbContext, true);
                con.AddParam("id", Id, SqlDbType.VarChar);
                con.AddOutputParam("errMsg", SqlDbType.VarChar);
                await con.executeNonQueryAsync("DeleteCategory");
                await con.commitAsync();
                return con.outPutParamVaLue["errMsg"].ToString();
            }
            catch (Exception ex)
            {
                await con.rollbackAsync();
                return ex.Message.ToString();
            }
            finally
            {
                await con.disposeAsync();
            }
        }
        public async Task<(List<CategoryModel>, string)> GetAllCategoryList()
        {
            DataAccessManager<AppDBContecxt> con = new DataAccessManager<AppDBContecxt>();
            List<CategoryModel> model = new List<CategoryModel>();
            try
            {
                await con.GetDataAccess(_dbContext);
                using (var reader = await con.executeAsReaderAsync("CategoryList"))
                {
                    while (reader.Read())
                    {
                        CategoryModel mod = new CategoryModel();
                        mod.Id = reader.GetInt32("id");
                        mod.Name = reader.GetString("categoryName");
                        model.Add(mod);
                    }
                }
                return (model, "");
            }
            catch (Exception ex)
            {
                await con.rollbackAsync();
                return (model, ex.Message);
            }
            finally
            {
                await con.disposeAsync();
            }
        }
    }
}
