namespace Api.Extensions;

public static class MediatrExtensions
{
    public static IServiceCollection AddMediatrConfiguration(
        this IServiceCollection services,
        Assembly assembly)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        return services;
    }
}
