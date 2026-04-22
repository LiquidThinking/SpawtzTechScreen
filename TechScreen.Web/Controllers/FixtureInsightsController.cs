using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechScreen.Web.Data;
using TechScreen.Web.Services;

namespace TechScreen.Web.Controllers;

public class FixtureInsightsController : Controller
{
    private readonly TenantContext _tenantContext;
    private readonly AppDbContext _dbContext;

    public FixtureInsightsController(TenantContext tenantContext, AppDbContext dbContext)
    {
        _tenantContext = tenantContext;
        _dbContext = dbContext;
    }

    [Route("FixtureInsights")]
    public async Task<IActionResult> Index()
    {
        // TODO: Replace stub SQL with real queries against the Fixtures table
        const string sql = @"
select
    0 as TotalFixtures,
    null as FirstGame,
    null as FinalGame;

select distinct
    Competition as [Name],
    0 as TeamCount,
    0 as FixtureCount,
    0 as RoundCount,
    null as FirstGame,
    null as FinalGame,
    null as DurationInDays
from
    Fixtures
";

        await using (var dbReader = await _dbContext
                         .Database
                         .GetDbConnection()
                         .QueryMultipleAsync(sql))
        {
            var overall = await dbReader.ReadSingleAsync<FixtureInsightsViewModel>();

            var seasons = (await dbReader.ReadAsync<FixtureInsightsViewModel.SeasonInsights>()).ToList();

            overall.TenantName = _tenantContext.FriendlyName;

            overall.Seasons = seasons;

            return View(overall);
        }
    }

    public class FixtureInsightsViewModel
    {
        public string TenantName { get; set; } = string.Empty;

        public int TotalFixtures { get; set; }

        public DateTime? FirstGame { get; set; }

        public DateTime? FinalGame { get; set; }

        public List<SeasonInsights> Seasons { get; set; } = new();

        public class SeasonInsights
        {
            public string Name { get; set; } = string.Empty;

            public int TeamCount { get; set; }

            public int FixtureCount { get; set; }

            public int RoundCount { get; set; }

            public DateTime? FirstGame { get; set; }

            public DateTime? FinalGame { get; set; }

            public int? DurationInDays { get; set; }
        }
    }
}
