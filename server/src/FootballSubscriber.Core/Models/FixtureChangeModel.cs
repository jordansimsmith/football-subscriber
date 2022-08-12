using System;

namespace FootballSubscriber.Core.Models;

public class FixtureChangeModel
{
    public string HomeTeam { get; set; }
    public string AwayTeam { get; set; }

    public DateTime OldDateTime { get; set; }
    public string OldVenue { get; set; }
    public string OldAddress { get; set; }

    public DateTime NewDateTime { get; set; }
    public string NewVenue { get; set; }
    public string NewAddress { get; set; }
}
