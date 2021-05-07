using System;
using System.Collections.Generic;

namespace FootballSubscriber.Core.Entities
{
    public class Fixture
    {
        /// <summary>
        ///     Local unique identifier
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        ///     Third party unique identifier
        /// </summary>
        public int ApiId { get; set; }

        /// <summary>
        ///     Set on save
        /// </summary>
        public int CompetitionApiId { get; set; }

        public int HomeTeamId { get; set; }
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
        
        public int CompetitionId { get; set; }
        public Competition Competition { get; set; }
    }
}