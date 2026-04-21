using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechScreen.Web.Data;
using TechScreen.Web.Services;

namespace TechScreen.Web.Controllers;

public class CompetitionController : Controller
{
    private readonly TenantContext _tenantContext;
    private readonly AppDbContext _dbContext;

    public CompetitionController(TenantContext tenantContext, AppDbContext dbContext)
    {
        _tenantContext = tenantContext;
        _dbContext = dbContext;
    }

    [Route("Competitions")]
    public async Task<IActionResult> Index()
    {
        var viewModel = new CompetitionsViewModel
        {
            TenantName = _tenantContext.FriendlyName,
            Competitions = new List<CompetitionsViewModel.Competition>()
        };

        const string sql = @"
select 
	compTeams.Competition as [Name], 
	COUNT( distinct Team ) as TeamCount
from 
	( 
		select 
			Competition,
			HomeTeam as Team
		from
			Fixtures

		union

		select
			Competition, 
			AwayTeam as Team
		from 
			Fixtures
	) as compTeams
group by
	compTeams.Competition
order by
	compTeams.Competition asc
";

        var competitions = await _dbContext
            .Database
            .GetDbConnection()
            .QueryAsync<dynamic>(sql);

        foreach (var competition in competitions)
        {
            viewModel.Competitions.Add(new CompetitionsViewModel.Competition
            {
                Name = (string)competition.Name,
                TeamCount = (int)competition.TeamCount
            });
        }

        return View(viewModel);
    }

    public class CompetitionsViewModel
    {
        public string TenantName { get; set; }

        public List<Competition> Competitions { get; set; }

        public class Competition
        {
            public string Name { get; set; }

            public int TeamCount { get; set; }
        }
    }
}