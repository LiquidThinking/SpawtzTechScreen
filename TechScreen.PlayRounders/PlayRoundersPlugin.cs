using Microsoft.AspNetCore.Html;
using TechScreen.Abstractions;

public class PlayRoundersPlugin : ITenantPlugin
{
    public string TenantId => "playrounders";

    public void ConfigureServices(ITenantServiceBuilder services)
    {
        services.AddScoped<IBrandingService, PlayRoundersBrandingService>();
    }
}

internal class PlayRoundersBrandingService : IBrandingService
{
    public async Task<IHtmlContent> GetLogo()
    {
        await using (var stream = typeof(PlayRoundersBrandingService)
                         .Assembly
                         .GetManifestResourceStream("TechScreen.PlayRounders.Assets.PlayRoundersLogo.svg")!)
        {
            using var reader = new StreamReader(stream);

            var svg = await reader.ReadToEndAsync();

            var base64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(svg));

            return new HtmlString($"""<img src="data:image/svg+xml;base64,{base64}" alt="Logo" height="32" />""");
        }
    }
}