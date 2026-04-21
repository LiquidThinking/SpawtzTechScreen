using TechScreen.Abstractions;

namespace TechScreen.Kent5aside;

public class Kent5asidePlugin : ITenantPlugin
{
    public string TenantId => "kent5aside";

    public void ConfigureServices(ITenantServiceBuilder services)
    {
        services.AddScoped<IFooterService, Kent5asideFooterService>();
    }
}

public class Kent5asideFooterService : IFooterService
{
    public Task<string> GetFooterText(DateOnly today)
    {
        return Task.FromResult($"Kent5aside - Kent's BEST game - {today.Year}");
    }
}
