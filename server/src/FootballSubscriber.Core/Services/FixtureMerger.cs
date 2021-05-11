using System.Threading.Tasks;
using FootballSubscriber.Core.Entities;
using FootballSubscriber.Core.Interfaces;

namespace FootballSubscriber.Core.Services
{
    public class FixtureMerger : MergerBase<Fixture>
    {
        private readonly IRepository<Fixture> _fixtureRepository;

        public FixtureMerger(IRepository<Fixture> fixtureRepository)
        {
            _fixtureRepository = fixtureRepository;
        }

        protected override int GetEntityComparableKey(Fixture entity)
        {
            return entity.ApiId;
        }

        protected override Task UpdateEntityAsync(Fixture oldEntity, Fixture newEntity)
        {
            // TODO: notify subscribers that fixtures have been updated
            oldEntity.CompetitionApiId = oldEntity.CompetitionApiId;
            oldEntity.Competition = oldEntity.Competition;

            oldEntity.HomeTeamApiId = newEntity.HomeTeamApiId;
            oldEntity.HomeTeamName = newEntity.HomeTeamName;
            oldEntity.HomeTeam = newEntity.HomeTeam;
            oldEntity.HomeOrganisationId = newEntity.HomeOrganisationId;
            oldEntity.HomeOrganisationLogo = newEntity.HomeOrganisationLogo;

            oldEntity.AwayTeamApiId = newEntity.HomeTeamApiId;
            oldEntity.AwayTeamName = newEntity.AwayTeamName;
            oldEntity.AwayTeam = newEntity.AwayTeam;
            oldEntity.AwayOrganisationId = newEntity.AwayOrganisationId;
            oldEntity.AwayOrganisationLogo = newEntity.AwayOrganisationLogo;
            
            oldEntity.Date = newEntity.Date;
            oldEntity.VenueId = newEntity.VenueId;
            oldEntity.VenueName = newEntity.VenueName;
            oldEntity.Address = newEntity.Address;

            return Task.CompletedTask;
        }

        protected override async Task InsertEntityAsync(Fixture newEntity)
        {
            await _fixtureRepository.AddAsync(newEntity);
        }

        protected override Task RemoveEntityAsync(Fixture oldEntity)
        {
            _fixtureRepository.Remove(oldEntity);

            return Task.CompletedTask;
        }

        protected override async Task OnMergeCompleteAsync()
        {
            await _fixtureRepository.SaveChangesAsync();
        }
    }
}