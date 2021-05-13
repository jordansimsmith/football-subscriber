using System.Text.Json.Serialization;

namespace FootballSubscriber.Core.Entities
{
    public class Subscription
    {
        public int? Id { get; set; }
        public string UserId { get; set; }

        [JsonIgnore] public Team Team { get; set; }

        public int TeamId { get; set; }
    }
}