namespace Api.Features.Products.Articles;

public static class ArticleForProductErrors
{
    public static Error NotFound(
        int id,
        int productId)
    {
        return new(
            Code: "ArticleForProduct.NotFound",
            Description: $"Article with Id='{id}' and ProductId='{productId}' not found");
    }

    public static Error ProductNotFound(
        int productId)
    {
        return new(
            Code: "ArticleForProduct.NotFound",
            Description: $"Product with Id='{productId}' not found");
    }
}
