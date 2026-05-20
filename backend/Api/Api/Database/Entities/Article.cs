namespace Api.Database.Entities;

public class Article
{
    public int Id { get; set; }

    public string? Barcode { get; set; }

    public string? Color { get; set; }

    public string? Size { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }



    #region FK
    public int ProductId { get; set; }

    public Product Product { get; set; } = default!;
    #endregion
}
