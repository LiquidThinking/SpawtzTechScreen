using TechScreen.Abstractions;

namespace TechScreen.SimpleCricket;

public class SimpleCricketPlugin : ITenantPlugin
{
    public string TenantId => "simplecricket";

    public void ConfigureServices(ITenantServiceBuilder services)
    {
        services.AddScoped<IFooterService, SimpleCricketFooterService>();
    }
}

public class SimpleCricketFooterService : IFooterService
{
    public Task<string> GetFooterText(DateOnly today)
    {
        return Task.FromResult("Simple Cricket, since 2010 &#x1F3CF;");
    }
}