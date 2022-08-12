using AutoMapper;
using FootballSubscriber.Core.Entities;
using FootballSubscriber.Core.Models;

namespace FootballSubscriber.Core.Mappers;

public class CompetitionProfile : Profile
{
    public CompetitionProfile()
    {
        CreateMap<CompetitionModel, Competition>()
            .ForMember(o => o.Id, cfg => cfg.Ignore())
            .ForMember(o => o.ApiId, cfg => cfg.MapFrom(o => o.Id));
    }
}
