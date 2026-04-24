using System.Numerics;
using Src.Shared.Base;

namespace Src.Shared.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : BaseModel
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(int id);
    Task CreateAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(BigInteger id);
}