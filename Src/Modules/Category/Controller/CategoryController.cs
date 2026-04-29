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
        public CategoryController(CategoryService service) : base(service)
        {
        }
    }
}