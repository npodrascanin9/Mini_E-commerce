namespace Api.Database.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int ProductCategoryId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ProductCategory ProductCategory { get; set; }
    public ICollection<Article> Articles { get; set; } = [];
}
