namespace Api.Features.ProductCategories;

public static class ProductCategoryErrors
{
    public static Error NotFound(int id)
        => new Error(
            Code: "ProductCategory.NotFound", 
            Description: $"Product category record with Id='{id}' not found");
}
