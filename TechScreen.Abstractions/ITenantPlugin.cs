namespace TechScreen.Abstractions;

public interface ITenantPlugin
{
    string TenantId { get; }

    void ConfigureServices(ITenantServiceBuilder services);
}
