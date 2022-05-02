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
            .ForMember(o => o.HomeTeamId, cfg => cfg.Ignore())
            .ForMember(o => o.AwayTeamId, cfg => cfg.Ignore())
            .ForMember(o => o.HomeTeamApiId, cfg => cfg.MapFrom(o => o.HomeTeamId))
            .ForMember(o => o.AwayTeamApiId, cfg => cfg.MapFrom(o => o.AwayTeamId));
    }
}