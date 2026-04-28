using Microsoft.AspNetCore.Mvc;
using Src.Shared.Interfaces;

namespace Src.Shared.Base
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController<TEntity, TCreateDTO, TUpdateDTO> : ControllerBase
        where TEntity : BaseModel
    {
        protected readonly IBaseService<TEntity, TCreateDTO, TUpdateDTO> _service;

        protected BaseController(IBaseService<TEntity, TCreateDTO, TUpdateDTO> service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAll();
            return Ok(result);
        }


        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(long id)
        {
            var result = await _service.GetById(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }


        [HttpPost("create")]
        public virtual async Task<IActionResult> Create([FromBody] TCreateDTO dto)
        {
            var result = await _service.Create(dto);
            return Ok(result);
        }


        [HttpPut("update")]
        public virtual async Task<IActionResult> Update([FromBody] TUpdateDTO dto)
        {
            var result = await _service.Update(dto);
            return Ok(result);
        }


        [HttpDelete("{id}/delete")]
        public virtual async Task<IActionResult> Delete(long id)
        {
            await _service.Delete(id);
            return Ok(new { success = true });
        }
    }
}