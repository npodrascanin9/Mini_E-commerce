namespace Api.IntegrationTests;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection Remove<TService>(
        this IServiceCollection services)
    {
        var serviceDescriptor = services.FirstOrDefault(
            descriptor => descriptor.ServiceType == typeof(TService));

        if (serviceDescriptor is not null)
        {
            services.Remove(serviceDescriptor);
        }

        return services;
    }
}
