using Microsoft.AspNetCore.Mvc;
using TechScreen.Web.Data;

namespace TechScreen.Web.Controllers;

public class HomeController : Controller
{
    private readonly IReadOnlyList<TenantConfig> _tenants;

    public HomeController(IReadOnlyList<TenantConfig> tenants)
    {
        _tenants = tenants;
    }

    public IActionResult Index()
    {
        return View(_tenants);
    }
}
