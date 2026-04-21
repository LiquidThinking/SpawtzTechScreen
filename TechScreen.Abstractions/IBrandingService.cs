using Microsoft.AspNetCore.Html;

namespace TechScreen.Abstractions;

public interface IBrandingService
{
    IHtmlContent Logo { get; }
}
