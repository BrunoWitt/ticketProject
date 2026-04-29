using Src.Shared.Base;
using Src.Modules.Category.Models;

namespace Src.Modules.Category.Repository
{
    public class CategoryRepository : BaseRepository<CategoryModel>, ICategoryRepository
    {
        public CategoryRepository(IConfiguration config) : base(config)
        {
        }

        protected override string TableName => "categoria";
    }
}