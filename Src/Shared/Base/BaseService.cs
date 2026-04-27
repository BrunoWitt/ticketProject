using Src.Shared.Interfaces;

namespace Src.Shared.Base
{
    public abstract class BaseService<TEntity, TCreateDTO, TUpdateDTO> 
        : IBaseService<TEntity, TCreateDTO, TUpdateDTO> 
        where TEntity : BaseModel, new()
    {
        protected readonly IBaseRepository<TEntity> _repo;

        protected BaseService(IBaseRepository<TEntity> repo)
        {
            _repo = repo;
        }


        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _repo.GetAllAsync();
        }

        public virtual async Task<TEntity?> GetById(long id)
        {
            return await _repo.GetByIdAsync((int)id);
        }


        public virtual async Task<TEntity> Create(TCreateDTO dto)
        {
            var entity = MapCreate(dto);

            await _repo.CreateAsync(entity);

            return entity;
        }


        public virtual async Task<TEntity> Update(TUpdateDTO dto)
        {
            var entity = MapUpdate(dto);

            await _repo.UpdateAsync(entity);

            return entity;
        }


        public virtual async Task Delete(long id)
        {
            await _repo.DeleteAsync(id);
        }


        protected abstract TEntity MapCreate(TCreateDTO dto);
        protected abstract TEntity MapUpdate(TUpdateDTO dto);
    }
}