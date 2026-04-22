using Microsoft.AspNetCore.Html;

namespace TechScreen.Abstractions;

public interface IBrandingService
{
    Task<IHtmlContent> GetLogo();
}
