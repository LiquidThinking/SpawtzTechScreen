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
    null as EarliestGame,
    null as LatestGame;

select
    'Stub' as Season,
    0 as FixtureCount,
    0 as RoundCount,
    null as EarliestGame,
    null as LatestGame,
    null as DurationInDays;
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

        public DateTime? EarliestGame { get; set; }

        public DateTime? LatestGame { get; set; }

        public List<SeasonInsights> Seasons { get; set; } = new();

        public class SeasonInsights
        {
            public string Season { get; set; } = string.Empty;

            public int FixtureCount { get; set; }

            public int RoundCount { get; set; }

            public DateTime? EarliestGame { get; set; }

            public DateTime? LatestGame { get; set; }

            public int? DurationInDays { get; set; }
        }
    }
}
