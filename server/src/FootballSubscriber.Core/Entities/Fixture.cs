using System;
using System.Text.Json.Serialization;
using FootballSubscriber.Core.Interfaces;

namespace FootballSubscriber.Core.Entities;

public class Fixture : IApiEntity
{
    /// <summary>
    /// Local unique identifier
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// Third party unique identifier
    /// </summary>
    [JsonIgnore]
    public long ApiId { get; set; }

    public int CompetitionId { get; set; }
    [JsonIgnore]
    public long CompetitionApiId { get; set; }

    [JsonIgnore]
    public Competition Competition { get; set; }

    public string HomeTeamName { get; set; }
    public string HomeOrganisationLogo { get; set; }
    public int? HomeScore { get; set; }

    public string AwayTeamName { get; set; }
    public string AwayOrganisationLogo { get; set; }
    public int? AwayScore { get; set; }

    public DateTime Date { get; set; }
    public string Status { get; set; }
    
    public string VenueName { get; set; }
    public string Address { get; set; }
    public decimal? Longitude { get; set; }
    public decimal? Latitude { get; set; }
}
