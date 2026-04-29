public class CreateCategoryDTO
{
    public string Nome { get; set; } = null!;
}

public class UpdateCategoryDTO
{
    public long Id { get; set; }
    public string? Nome { get; set; }
}