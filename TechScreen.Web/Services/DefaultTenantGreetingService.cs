using TechScreen.Abstractions;

namespace TechScreen.Web.Services;

public class DefaultFooterService : IFooterService
{
    private TenantContext TenantContext { get; }

    public DefaultFooterService(TenantContext tenantContext)
    {
        TenantContext = tenantContext;
    }

    public Task<string> GetFooterText(DateOnly today)
    {
        return Task.FromResult($"{TenantContext.FriendlyName} - {today.Year}");
    }
}