namespace Api.Features.ProductCategories;

public static class ProductCategoryErrors
{
    public static Error NotFound(int id)
        => new Error(
            Code: "ProductCategory.NotFound", 
            Description: $"Product category record with Id='{id}' not found");

    public static Error ContainsProducts(
        int id)
    {
        return new(
            Code: "ProductCategory.BadRequest",
            Description: $"Category with Id='{id}' cannot be deleted since it contains products");
    }

    public static Error NameAlreadyExists(
        string name)
    {
        return new Error(
            "ProductCategory.BadRequest",
            $"Category with Name='{name}' already exists");
    }
}
