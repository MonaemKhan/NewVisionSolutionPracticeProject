using Product.API.DBConnection;
using Product.Model;
using System.Data;

namespace Product.API.Repos
{
    public class ProductService : IProductService
    {
        private readonly AppDBContecxt _dbContext;

        public ProductService(AppDBContecxt dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(ProductModel, string)> InsertUpdateProduct(ProductModel _product)
        {
            DataAccessManager<AppDBContecxt> con = new DataAccessManager<AppDBContecxt>();
            try
            {
                await con.GetDataAccess(_dbContext, true);
                con.AddParam("name", _product.Name, SqlDbType.VarChar);
                con.AddParam("category", _product.CategoryId, SqlDbType.Int);
                con.AddParam("price", _product.price, SqlDbType.Decimal);
                con.AddParam("stockquentity", _product.StockQuentity, SqlDbType.Int);
                con.AddParam("lastUpdate", _product.LastUpdate, SqlDbType.DateTimeOffset);
                con.AddParam("id", _product.Id, SqlDbType.Int, ParameterDirection.InputOutput);
                con.AddOutputParam("errMsg", SqlDbType.VarChar);
                await con.executeNonQueryAsync("InsertUpdateProduct");
                if (!string.IsNullOrEmpty(con.outPutParamVaLue["errMsg"].ToString()))
                {
                    _product.Id = 0;
                    return (_product, con.outPutParamVaLue["errMsg"].ToString());
                }
                _product.Id = Convert.ToInt32(con.outPutParamVaLue["id"].ToString());
                await con.commitAsync();
                return (_product,con.outPutParamVaLue["errMsg"].ToString());
            }
            catch (Exception ex)
            {
                await con.rollbackAsync();
                return (_product, ex.Message.ToString());
            }
            finally
            {
                await con.disposeAsync();
            }
        }
        public async Task<string> DeleteProduct(int Id)
        {
            DataAccessManager<AppDBContecxt> con = new DataAccessManager<AppDBContecxt>();
            try
            {
                await con.GetDataAccess(_dbContext, true);
                con.AddParam("id", Id, SqlDbType.VarChar);
                con.AddOutputParam("errMsg", SqlDbType.VarChar);
                await con.executeNonQueryAsync("deleteProduct");
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
        public async Task<(List<ProductModel>, string)> GetAllProductList()
        {
            DataAccessManager<AppDBContecxt> con = new DataAccessManager<AppDBContecxt>();
            List<ProductModel> model = new List<ProductModel>();
            try
            {
                await con.GetDataAccess(_dbContext);
                using (var reader = await con.executeAsReaderAsync("getProductList"))
                {
                    while (reader.Read())
                    {
                        ProductModel mod = new ProductModel();
                        mod.Id = reader.GetInt32("id");
                        mod.Name = reader.GetString("name");
                        mod.price = reader.GetDecimal("price");
                        mod.StockQuentity = reader.GetInt32("stockQuantity");
                        mod.CategoryId = reader.GetInt32("Category");
                        mod.CategoryName = reader.GetString("CategoryName");
                        try
                        {
                            mod.LastUpdate = reader.GetDateTime("LastUpdateDate").ToString("dd-MMM-yyyy");
                        }
                        catch { }
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
