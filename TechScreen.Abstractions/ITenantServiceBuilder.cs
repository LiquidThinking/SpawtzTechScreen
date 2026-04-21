namespace TechScreen.Abstractions;

public interface ITenantServiceBuilder
{
    ITenantServiceBuilder AddScoped<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService;
}
