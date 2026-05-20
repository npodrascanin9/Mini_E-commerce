namespace Api.Abstractions.ErrorResponse;

public sealed record Error(
    string Code, 
    string? Description = default)
{
    public static readonly Error None = new(
        Code: string.Empty);

    public static readonly Error Null = new(
        Code: "Error.NullValue",
        Description: "The specified result value is null.");

    public static implicit operator Result(
        Error error) => Result.Failure(error);
}
