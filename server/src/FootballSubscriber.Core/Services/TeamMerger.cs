using System.Threading.Tasks;
using FootballSubscriber.Core.Entities;
using FootballSubscriber.Core.Interfaces;

namespace FootballSubscriber.Core.Services;

public class TeamMerger : MergerBase<Team>
{
    private readonly IRepository<Team> _teamRepository;

    public TeamMerger(IRepository<Team> teamRepository)
    {
        _teamRepository = teamRepository;
    }

    protected override long GetEntityComparableKey(Team entity)
    {
        return entity.ApiId;
    }

    protected override Task UpdateEntityAsync(Team oldEntity, Team newEntity)
    {
        oldEntity.Name = newEntity.Name;

        return Task.CompletedTask;
    }

    protected override async Task InsertEntityAsync(Team newEntity)
    {
        await _teamRepository.AddAsync(newEntity);
    }

    protected override Task RemoveEntityAsync(Team oldEntity)
    {
        // Intentionally left blank, teams should not be deleted as part of the merging process

        return Task.CompletedTask;
    }

    protected override async Task OnMergeCompleteAsync()
    {
        await _teamRepository.SaveChangesAsync();
    }
}