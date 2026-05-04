using Src.Shared.Base;
using Src.Modules.Ticket.Models;

namespace Src.Modules.Historico.Models;

public class HistoricoModel : BaseModel
{
    public long IdTicket { get; set; }

    public StatusTicket StatusAnterior { get; set; }
    public StatusTicket StatusNovo { get; set; }

    public DateTime DataAlteracao { get; set; }
}