namespace TechScreen.Web.Data;

public class Fixture
{
    public int Id { get; set; }

    public string Competition { get; set; }

    public int Round { get; set; }

    public DateTime DateTime { get; set; }

    public string Venue { get; set; }

    public string Pitch { get; set; }

    public string HomeTeam { get; set; }

    public string AwayTeam { get; set; }

    public decimal HomeTeamScore { get; set; }

    public decimal AwayTeamScore { get; set; }
}