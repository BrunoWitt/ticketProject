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