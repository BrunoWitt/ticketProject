using Src.Shared.Base;

namespace Src.Modules.Message.Models;

public class MessageModel : BaseModel
{
    public string Texto { get; set; } = string.Empty;

    public long IdUsuario { get; set; }
    public long IdTicket { get; set; }
}


public class AnexoModel
{
    public long Id { get; set; }
    public required byte[] Arquivo { get; set; }
    public required string Tipo { get; set; }
    public long IdMensagem { get; set; }
    public DateTime DataHoraCriado { get; set; }
}