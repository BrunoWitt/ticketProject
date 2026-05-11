using Src.Modules.Category.Models;
using Src.Modules.Category.Repository;
using Src.Shared.Base;
using Src.Modules.Ticket.Repository;

namespace Src.Modules.Category.Service
{
    public class CategoryService
        : BaseService<CategoryModel, CreateCategoryDTO, UpdateCategoryDTO>
    {
        private readonly ITicketRepository _ticketRepository;

        public CategoryService(
            ICategoryRepository repo,
            ITicketRepository ticketRepository
        ) : base(repo)
        {
            _ticketRepository = ticketRepository;
        }


        public async Task DeleteCategory(long id)
        {
            var tickets = await _ticketRepository.GetAllAsync();

            var hasTickets = tickets.Any(t =>
                t.IdCategoria == id &&
                t.DataHoraDelecao == null
            );

            if (hasTickets)
                throw new Exception(
                    "Não é possível deletar categoria com tickets vinculados"
                );

            await _repo.DeleteAsync((int)id);
        }

        protected override CategoryModel MapCreate(CreateCategoryDTO dto)
        {
            return new CategoryModel
            {
                Nome = dto.Nome!,
                Cor = dto.Cor,
                DataHoraCriado = DateTimeOffset.UtcNow
            };
        }

        protected override CategoryModel MapUpdate(UpdateCategoryDTO dto)
        {
            return new CategoryModel
            {
                Id = dto.Id,
                Nome = dto.Nome ?? "",
                Cor = dto.Cor
            };
        }
    }
}