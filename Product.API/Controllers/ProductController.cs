using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Product.API.Repos;
using Product.Model;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            APIReponce<List<ProductModel>> responce = new APIReponce<List<ProductModel>>();
            (responce.ResponceData, responce.ErrorMsg) = await _productService.GetAllProductList();
            if (!string.IsNullOrEmpty(responce.ErrorMsg))
            {
                return BadRequest(responce);
            }
            return Ok(responce);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ProductModel data)
        {
            APIReponce<ProductModel> responce = new APIReponce<ProductModel>();
            (responce.ResponceData, responce.ErrorMsg) = await _productService.InsertUpdateProduct(data);
            if (!string.IsNullOrEmpty(responce.ErrorMsg))
            {
                return BadRequest(responce.ErrorMsg);
            }
            return Ok(responce.ResponceData);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            var err = await _productService.DeleteProduct(Id);
            if (!string.IsNullOrEmpty(err))
            {
                return BadRequest(err);
            }
            return Ok();
        }
    }
}
