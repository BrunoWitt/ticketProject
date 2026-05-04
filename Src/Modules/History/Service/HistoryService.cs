using Src.Modules.Historico.Models;
using Src.Modules.Historico.Repository;
using Src.Modules.Ticket.Models;

namespace Src.Modules.Historico.Service;

public class HistoricoService
{
    private readonly IHistoricoRepository _repo;

    public HistoricoService(IHistoricoRepository repo)
    {
        _repo = repo;
    }

    public async Task RegistrarMudanca(
        long idTicket,
        StatusTicket anterior,
        StatusTicket novo)
    {
        var historico = new HistoricoModel
        {
            IdTicket = idTicket,
            StatusAnterior = anterior,
            StatusNovo = novo,
            DataAlteracao = DateTime.UtcNow
        };

        await _repo.CreateAsync(historico);
    }

    public async Task<List<HistoricoModel>> GetByTicket(long idTicket)
    {
        return await _repo.GetByTicket(idTicket);
    }
}