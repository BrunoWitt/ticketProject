namespace Src.Shared.Base;

public abstract class BaseModel
{
    public int Id {get;set;}

    public DateTimeOffset DataHoraCriado {get;set;}

    public DateTimeOffset DataHoraDelecao {get;set;}
}