using Microsoft.AspNetCore.Html;
using TechScreen.Abstractions;

namespace TechScreen.Web.Services;

public class DefaultBrandingService : IBrandingService
{
    public Task<IHtmlContent> GetLogo()
    {
        const string spawtzLogo = @"<img src=""/Spawtz.svg"" alt=""Logo"" height=""32"" />";

        return Task.FromResult<IHtmlContent>(new HtmlString(spawtzLogo));
    }
}
