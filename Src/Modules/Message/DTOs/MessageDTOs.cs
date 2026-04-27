public class AnexoDTO
{
    public int IdMensagem {get;set;}
    public IFormFile? Arquivo {get;set;}
}


public class MenssageDTO
{
    public string? Texto {get;set;}
    public int IdUsuario {get;set;}
    public int IdTicket {get;set;}
}


public class CreateMessageDTO
{
    public string? Texto { get; set; }
    public long IdUsuario { get; set; }
    public long IdTicket { get; set; }
}

public class UpdateMessageDTO
{
    public long Id { get; set; }
    public string? Texto { get; set; }
}