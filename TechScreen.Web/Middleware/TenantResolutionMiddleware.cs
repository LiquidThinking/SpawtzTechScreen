using TechScreen.Web.Data;
using TechScreen.Web.Services;

namespace TechScreen.Web.Middleware;

public class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _baseDomain;
    private readonly IReadOnlyList<TenantConfig> _tenants;

    public TenantResolutionMiddleware(RequestDelegate next, IConfiguration config, IReadOnlyList<TenantConfig> tenants)
    {
        _next = next;
        _baseDomain = config["Multitenancy:BaseDomain"] ?? "localhost";
        _tenants = tenants;
    }

    public async Task InvokeAsync(HttpContext context, TenantContext tenantContext)
    {
        var host = context.Request.Host.Host;

        var parts = host.Split('.');

        if (parts.Length >= 2 && parts[0] != _baseDomain)
        {
            tenantContext.TenantId = parts[0];

            var config = _tenants.FirstOrDefault(t => t.TenantId == parts[0]);

            if (config != null)
            {
                tenantContext.FriendlyName = config.FriendlyName;
                tenantContext.Theme = config.Theme;
            }
        }

        await _next(context);
    }
}
