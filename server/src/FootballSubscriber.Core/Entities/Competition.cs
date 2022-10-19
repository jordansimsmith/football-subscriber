using System.Collections.Generic;
using System.Text.Json.Serialization;
using FootballSubscriber.Core.Interfaces;

namespace FootballSubscriber.Core.Entities;

public class Competition : IApiEntity
{
    /// <summary>
    ///     Local unique identifier
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    ///     Third party unique identifier
    /// </summary>
    [JsonIgnore]
    public long ApiId { get; set; }

    public string Name { get; set; }

    [JsonIgnore]
    public ICollection<Fixture> Fixtures { get; set; }
}
