using System;
using System.Collections.Generic;

namespace FootballSubscriber.Core.Models;

public class GetFixturesResponseModel
{
    public DateTime FirstFixtureDate { get; set; }
    public IEnumerable<FixtureModel> Fixtures { get; set; }
    public DateTime LastResultDate { get; set; }
    public IEnumerable<RoundModel> RoundInfo { get; set; }
}
