using Src.Modules.Category.Models;
using Src.Modules.Category.Repository;
using Src.Shared.Base;

namespace Src.Modules.Category.Service
{
    public class CategoryService
        : BaseService<CategoryModel, CreateCategoryDTO, UpdateCategoryDTO>
    {
        public CategoryService(ICategoryRepository repo) : base(repo)
        {
        }

        protected override CategoryModel MapCreate(CreateCategoryDTO dto)
        {
            return new CategoryModel
            {
                Nome = dto.Nome!,
                DataHoraCriado = DateTimeOffset.UtcNow
            };
        }

        protected override CategoryModel MapUpdate(UpdateCategoryDTO dto)
        {
            return new CategoryModel
            {
                Id = dto.Id,
                Nome = dto.Nome ?? ""
            };
        }
    }
}