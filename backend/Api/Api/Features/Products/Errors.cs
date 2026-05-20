namespace Api.Features.Products;

public static class ProductErrors
{
    public static Error NameAlreadyExists(
        string productName)
    {
        return new Error(
            Code: "Products.BadRequest",
            Description: $"Product Name='{productName}' already exists");
    }

    public static Error NotFound(
        int id)
    {
        return new Error(
            Code: "Products.NotFound",
            Description: $"Product with Id='{id}' not found");
    }

    public static Error ProductCategoryNotFound(
        int productCategoryId)
    {
        return new Error(
            Code: "Products.NotFound",
            Description: $"Product category with Id='{productCategoryId}' not found");
    }

    public static Error Rollback(
        string errorMessage)
    {
        return new Error(
            Code: "Products.BadRequest",
            Description: errorMessage);
    }
}
