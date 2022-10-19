using AutoMapper;
using FootballSubscriber.Core.Entities;
using FootballSubscriber.Core.Models;

namespace FootballSubscriber.Core.Mappers;

public class FixtureProfile : Profile
{
    public FixtureProfile()
    {
        // set the third party Id as ApiId
        CreateMap<FixtureModel, Fixture>()
            .ForMember(o => o.Id, cfg => cfg.Ignore())
            .ForMember(o => o.ApiId, cfg => cfg.MapFrom(o => o.Id))
            .ForMember(o => o.CompetitionId, cfg => cfg.Ignore())
            .ForMember(o => o.HomeTeamName, cfg => cfg.MapFrom(o => o.HomeTeamNameAbbr))
            .ForMember(o => o.HomeOrganisationLogo, cfg => cfg.MapFrom(o => o.HomeOrgLogo))
            .ForMember(o => o.AwayTeamName, cfg => cfg.MapFrom(o => o.AwayTeamNameAbbr))
            .ForMember(o => o.AwayOrganisationLogo, cfg => cfg.MapFrom(o => o.AwayOrgLogo));
    }
}
