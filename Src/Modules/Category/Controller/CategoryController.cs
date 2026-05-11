using Microsoft.AspNetCore.Mvc;
using Src.Modules.Category.Models;
using Src.Modules.Category.Service;
using Src.Shared.Base;

namespace Src.Modules.Category.Controller
{
    [ApiController]
    [Route("category")]
    public class CategoryController 
        : BaseController<CategoryModel, CreateCategoryDTO, UpdateCategoryDTO>
    {
        private readonly CategoryService _categoryService;
        public CategoryController(CategoryService service) : base(service)
        {
            _categoryService = service;
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(long id)
        {
            await _categoryService.DeleteCategory(id);

            return Ok(new
            {
                message = "Categoria deletada com sucesso"
            });
        }
    }
}