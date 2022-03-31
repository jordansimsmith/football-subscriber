using Newtonsoft.Json;

namespace FootballSubscriber.Infrastructure.Models
{
    public class FixtureChangeTemplateData
    {
        [JsonProperty("subject")] public string Subject { get; set; }
        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("home_team")] public string HomeTeam { get; set; }

        [JsonProperty("away_team")] public string AwayTeam { get; set; }

        [JsonProperty("old_date")] public string OldDate { get; set; }

        [JsonProperty("old_time")] public string OldTime { get; set; }

        [JsonProperty("old_venue")] public string OldVenue { get; set; }

        [JsonProperty("old_address")] public string OldAddress { get; set; }

        [JsonProperty("new_date")] public string NewDate { get; set; }

        [JsonProperty("new_time")] public string NewTime { get; set; }

        [JsonProperty("new_venue")] public string NewVenue { get; set; }

        [JsonProperty("new_address")] public string NewAddress { get; set; }

        [JsonProperty("app_url")] public string ApplicationUrl { get; set; }
    }
}