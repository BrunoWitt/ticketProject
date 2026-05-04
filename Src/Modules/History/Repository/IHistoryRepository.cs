using Src.Shared.Interfaces;
using Src.Modules.Historico.Models;

namespace Src.Modules.Historico.Repository;

public interface IHistoricoRepository : IBaseRepository<HistoricoModel>
{
    Task<List<HistoricoModel>> GetByTicket(long ticketId);
}