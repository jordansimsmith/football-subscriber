using System;
using System.Text.Json.Serialization;
using FootballSubscriber.Core.Interfaces;

namespace FootballSubscriber.Core.Entities;

public class Fixture: IApiEntity
{
    /// <summary>
    ///     Local unique identifier
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    ///     Third party unique identifier
    /// </summary>
    [JsonIgnore]
    public int ApiId { get; set; }

    /// <summary>
    ///     Set on save
    /// </summary>
    [JsonIgnore]
    public int CompetitionApiId { get; set; }

    public int HomeTeamId { get; set; }
    [JsonIgnore] public int HomeTeamApiId { get; set; }
    public string HomeTeamName { get; set; }
    [JsonIgnore] public Team HomeTeam { get; set; }
    public int HomeOrganisationId { get; set; }
    public string HomeOrganisationLogo { get; set; }

    public int AwayTeamId { get; set; }
    [JsonIgnore] public int AwayTeamApiId { get; set; }
    [JsonIgnore] public Team AwayTeam { get; set; }
    public string AwayTeamName { get; set; }
    public int AwayOrganisationId { get; set; }
    public string AwayOrganisationLogo { get; set; }

    public DateTime Date { get; set; }
    public int VenueId { get; set; }
    public string VenueName { get; set; }
    public string Address { get; set; }

    public int CompetitionId { get; set; }
    [JsonIgnore] public Competition Competition { get; set; }
}