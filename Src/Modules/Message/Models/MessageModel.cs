using Src.Shared.Base;

namespace Src.Modules.Message.Models;

public class MessageModel : BaseModel
{
    public string Texto { get; set; } = string.Empty;

    public int IdUsuario { get; set; }
    public int IdTicket { get; set; }
}