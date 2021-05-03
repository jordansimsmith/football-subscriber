using AutoMapper;
using FootballSubscriber.Core.Entities;
using FootballSubscriber.Core.Models;

namespace FootballSubscriber.Core.Mappers
{
    public class FixtureProfile : Profile
    {
        public FixtureProfile()
        {
            // set the third party Id as ApiId
            CreateMap<FixtureModel, Fixture>()
                .ForMember(o => o.Id, cfg => cfg.Ignore())
                .ForMember(o => o.ApiId, cfg => cfg.MapFrom(o => o.Id));
        }
    }
}