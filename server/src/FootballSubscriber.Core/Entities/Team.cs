using System.Collections.Generic;
using System.Text.Json.Serialization;
using FootballSubscriber.Core.Interfaces;

namespace FootballSubscriber.Core.Entities
{
    public class Team: IApiEntity
    {
        /// <summary>
        /// Local unique identifier
        /// </summary>
        public int? Id { get; set; }
        
        /// <summary>
        /// Third party unique identifier
        /// </summary>
        [JsonIgnore]
        public int ApiId { get; set; }
        
        public string Name { get; set; }

        [JsonIgnore] public ICollection<Fixture> HomeFixtures { get; set; }
        [JsonIgnore] public ICollection<Fixture> AwayFixtures { get; set; }
    }
}