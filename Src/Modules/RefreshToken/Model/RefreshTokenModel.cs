using Src.Shared.Base;

public class RefreshTokenModel : BaseModel
{
    public string Token { get; set; } = null!;
    public long IdUsuario { get; set; }

    public DateTime ExpiraEm { get; set; }
    public DateTime CriadoEm { get; set; }

    public DateTime? RevogadoEm { get; set; }
    public string? SubstituidoPor { get; set; }
}