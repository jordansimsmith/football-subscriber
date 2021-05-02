using System;

namespace FootballSubscriber.Core.Entities
{
    public class Fixture
    {
        public int Id { get; set; }
        
        public int HomeTeamId {get; set; }
        public string HomeTeamName { get; set; }
        public int HomeOrganisationId { get; set; }
        public string HomeOrganisationLogo { get; set; }

        public int AwayTeamId { get; set; }
        public string AwayTeamName { get; set; }
        public int AwayOrganisationId { get; set; }
        public string AwayOrganisationLogo { get; set; }
        
        public DateTime Date { get; set; }
        public int VenueId { get; set; }
        public string VenueName { get; set; }
        public string Address { get; set; }
    }
}