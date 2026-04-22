using TechScreen.Abstractions;

namespace TechScreen.SimpleCricket;

public class SimpleCricketPlugin : ITenantPlugin
{
    public string TenantId => "simplecricket";

    public void ConfigureServices(ITenantServiceBuilder services)
    {
    }
}