using System;
using System.Collections.Generic;

namespace FootballSubscriber.Core.Models
{
    public class FixtureModel
    {
        public string Id { get; set; }
        public object Number { get; set; }
        public bool IsSuperForm { get; set; }
        public string HomeOrganisationId { get; set; }
        public string HomeOrganisationName { get; set; }
        public string HomeOrganisationLogo { get; set; }
        public string AwayOrganisationId { get; set; }
        public string AwayOrganisationName { get; set; }
        public string AwayOrganisationLogo { get; set; }
        public string SportId { get; set; }
        public string SportName { get; set; }
        public string GradeId { get; set; }
        public string GradeName { get; set; }
        public object SeasonId { get; set; }
        public object SeasonName { get; set; }
        public string HomeTeamId { get; set; }
        public string HomeTeamName { get; set; }
        public object HomeTeamNameAbbr { get; set; }
        public string AwayTeamId { get; set; }
        public string AwayTeamName { get; set; }
        public object AwayTeamNameAbbr { get; set; }
        public object CompetitionId { get; set; }
        public object CompetitionName { get; set; }
        public string Round { get; set; }
        public string RoundName { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public string VenueId { get; set; }
        public string VenueName { get; set; }
        public string VenueNameAbbr { get; set; }
        public object VenueAddress { get; set; }
        public object GLN { get; set; }
        public object LocationId { get; set; }
        public object LocationName { get; set; }
        public object LocationLat { get; set; }
        public object LocationLng { get; set; }
        public string HomeScore { get; set; }
        public string AwayScore { get; set; }
        public object FixtureOfficials { get; set; }
        public int Provider { get; set; }
        public bool PublishVenue { get; set; }
        public bool PublishResults { get; set; }
        public bool PublishTeamsheetOnWidget { get; set; }
        public int SectionId { get; set; }
        public object SectionName { get; set; }
        public int GradeSortOrder { get; set; }
        public int SectionSortOrder { get; set; }
        public object PublicNotes { get; set; }
        public object StatusName { get; set; }
        public object CssName { get; set; }
        public int ResultStatus { get; set; }
        public bool InGame { get; set; }
        public object MatchDayDescription { get; set; }
        public object MatchNotes { get; set; }
        public object MatchSummary { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public object TeamAOrganisationId { get; set; }
        public object TeamBOrganisationId { get; set; }
        public int MatchDay { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Address { get; set; }
        public List<object> MatchOfficials { get; set; }
    }
}