public class CreateCategoryDTO
{
    public string? Nome { get; set; } 
    public string? Cor {get;set;}
}

public class UpdateCategoryDTO
{
    public long Id { get; set; }
    public string? Nome { get; set; }
    public string? Cor {get;set;}
}