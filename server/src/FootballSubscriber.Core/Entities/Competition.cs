using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FootballSubscriber.Core.Entities
{
    public class Competition
    {
        /// <summary>
        ///     Local unique identifier
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        ///     Third party unique identifier
        /// </summary>
        public int ApiId { get; set; }

        public string Name { get; set; }

        [JsonIgnore] public ICollection<Fixture> Fixtures { get; set; }
    }
}