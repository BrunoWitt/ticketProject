using System.Collections.Generic;
using System.Threading.Tasks;

namespace Src.Shared.Interfaces
{
    public interface IBaseService<TEntity, TCreateDTO, TUpdateDTO>
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity?> GetById(long id);
        Task<TEntity> Create(TCreateDTO dto);
        Task<TEntity> Update(TUpdateDTO dto);
        Task Delete(long id);
    }
}