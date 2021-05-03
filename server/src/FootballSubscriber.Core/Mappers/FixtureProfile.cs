using AutoMapper;
using FootballSubscriber.Core.Entities;
using FootballSubscriber.Core.Models;

namespace FootballSubscriber.Core.Mappers
{
    public class FixtureProfile : Profile
    {
        public FixtureProfile()
        {
            CreateMap<FixtureModel, Fixture>()
                .ConstructUsing(fixtureModel => new Fixture
                {
                    ApiId = int.Parse(fixtureModel.Id),
                    HomeTeamId = int.Parse(fixtureModel.HomeTeamId),
                    HomeTeamName = fixtureModel.HomeTeamName,
                    HomeOrganisationId = int.Parse(fixtureModel.HomeOrganisationId),
                    HomeOrganisationLogo = fixtureModel.HomeOrganisationLogo,
                    AwayTeamId = int.Parse(fixtureModel.AwayTeamId),
                    AwayTeamName = fixtureModel.AwayTeamName,
                    AwayOrganisationId = int.Parse(fixtureModel.AwayOrganisationId),
                    AwayOrganisationLogo = fixtureModel.AwayOrganisationLogo,
                    Date = fixtureModel.Date,
                    VenueId = int.Parse(fixtureModel.VenueId),
                    VenueName = fixtureModel.VenueName,
                    Address = fixtureModel.Address
                });
        }
    }
}