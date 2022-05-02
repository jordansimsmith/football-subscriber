using System.Threading.Tasks;
using FluentAssertions;
using FootballSubscriber.Core.Entities;
using FootballSubscriber.Core.Interfaces;
using FootballSubscriber.Core.Services;
using Moq;
using Xunit;

namespace FootballSubscriber.Test.Core;

public class TeamMergerTest
{
    private readonly Mock<IRepository<Team>> _mockTeamRepository;

    private readonly TeamMerger _subject;

    public TeamMergerTest()
    {
        _mockTeamRepository = new Mock<IRepository<Team>>();
            
        _subject = new TeamMerger(_mockTeamRepository.Object);
    }

    [Fact]
    public async Task MergeAsync_Should_Merge_Teams_Correctly()
    {
        // arrange
        var oldTeams = new[]
        {
            new Team
            {
                ApiId = 1,
                Name = "name 1"
            },
            new Team
            {
                ApiId = 3,
                Name = "name 3"
            },
            new Team
            {
                ApiId = 4,
                Name = "name 4"
            }
        };

        var newTeams = new[]
        {
            new Team
            {
                ApiId = 1,
                Name = "name 1 updated"
            },
            new Team
            {
                ApiId = 2,
                Name = "name 2"
            },
            new Team
            {
                ApiId = 5,
                Name = "name 5"
            }
        };

        // act
        await _subject.MergeAsync(oldTeams, newTeams);

        // assert
        _mockTeamRepository.Verify(x => x.AddAsync(It.Is<Team>(c => c.ApiId == 2)), Times.Once);
        _mockTeamRepository.Verify(x => x.AddAsync(It.Is<Team>(c => c.ApiId == 5)), Times.Once);
        _mockTeamRepository.Verify(x => x.Remove(It.IsAny<Team>()), Times.Never);

        _mockTeamRepository.Verify(x => x.SaveChangesAsync(), Times.Once);

        oldTeams[0].Name.Should().Be(newTeams[0].Name);
    }
}