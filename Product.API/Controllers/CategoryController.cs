using Microsoft.AspNetCore.Mvc;
using Product.API.Repos;
using Product.Model;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            APIReponce<List<CategoryModel>> responce = new APIReponce<List<CategoryModel>>();
            (responce.ResponceData, responce.ErrorMsg) = await _categoryService.GetAllCategoryList();
            if (!string.IsNullOrEmpty(responce.ErrorMsg))
            {
                return BadRequest(responce);
            }
            return Ok(responce);
        }
        [HttpPost]
        public async Task<IActionResult> Post(CategoryModel data)
        {
            APIReponce<CategoryModel> responce = new APIReponce<CategoryModel>();
            (responce.ResponceData, responce.ErrorMsg) = await _categoryService.InsertCategory(data);
            if (!string.IsNullOrEmpty(responce.ErrorMsg))
            {
                return BadRequest(responce.ErrorMsg);
            }
            return Ok(responce.ResponceData);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            var err = await _categoryService.DeleteCategory(Id);
            if (!string.IsNullOrEmpty(err))
            {
                return BadRequest(err);
            }
            return Ok();
        }
    }
}
