namespace FootballSubscriber.Core.Models
{
    public class CompetitionModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public int Provider { get; set; }
        public string SeasonId { get; set; }
        public string SeasonName { get; set; }
        public string SportId { get; set; }
        public string SportName { get; set; }
    }
}