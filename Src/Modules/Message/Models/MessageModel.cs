using Src.Shared.Base;

namespace Src.Modules.Message.Models;

public class MessageModel : BaseModel
{
    public string Texto { get; set; } = string.Empty;

    public int IdUsuario { get; set; }
    public int IdTicket { get; set; }
}


public class AnexoModel
{
    public int Id { get; set; }
    public required byte[] Arquivo { get; set; }
    public required string Tipo { get; set; }
    public int IdMensagem { get; set; }
    public DateTime DataHoraCriado { get; set; }
}