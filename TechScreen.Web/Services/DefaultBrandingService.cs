using Microsoft.AspNetCore.Html;
using TechScreen.Abstractions;

namespace TechScreen.Web.Services;

public class DefaultBrandingService : IBrandingService
{
    public IHtmlContent Logo { get; } = new HtmlString("""<img src="/Spawtz.svg" alt="Logo" height="32" />""");
}
