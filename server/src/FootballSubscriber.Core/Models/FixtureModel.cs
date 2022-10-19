using System;

namespace FootballSubscriber.Core.Models;

public class FixtureModel
{
    public long Id { get; set; }

    public string HomeTeamNameAbbr { get; set; }
    public string HomeOrgLogo { get; set; }
    public int? HomeScore { get; set; }

    public string AwayTeamNameAbbr { get; set; }
    public string AwayOrgLogo { get; set; }
    public int? AwayScore { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }

    public string VenueName { get; set; }
    public string Address { get; set; }
    public decimal? Longitude { get; set; }
    public decimal? Latitude { get; set; }
}
