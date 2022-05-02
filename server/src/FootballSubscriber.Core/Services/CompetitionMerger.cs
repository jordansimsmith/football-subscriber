using System.Threading.Tasks;
using FootballSubscriber.Core.Entities;
using FootballSubscriber.Core.Interfaces;

namespace FootballSubscriber.Core.Services;

public class CompetitionMerger : MergerBase<Competition>
{
    private readonly IRepository<Competition> _competitionRepository;

    public CompetitionMerger(IRepository<Competition> competitionRepository)
    {
        _competitionRepository = competitionRepository;
    }

    protected override int GetEntityComparableKey(Competition entity)
    {
        return entity.ApiId;
    }

    protected override Task UpdateEntityAsync(Competition oldEntity, Competition newEntity)
    {
        oldEntity.Name = newEntity.Name;

        return Task.CompletedTask;
    }

    protected override async Task InsertEntityAsync(Competition newEntity)
    {
        await _competitionRepository.AddAsync(newEntity);
    }

    protected override Task RemoveEntityAsync(Competition oldEntity)
    {
        _competitionRepository.Remove(oldEntity);

        return Task.CompletedTask;
    }

    protected override async Task OnMergeCompleteAsync()
    {
        await _competitionRepository.SaveChangesAsync();
    }
}