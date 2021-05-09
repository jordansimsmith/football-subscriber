using System.Threading.Tasks;
using FluentAssertions;
using FootballSubscriber.Core.Entities;
using FootballSubscriber.Core.Interfaces;
using FootballSubscriber.Core.Services;
using Moq;
using Xunit;

namespace FootballSubscriber.Test.Core
{
    public class CompetitionMergerTest
    {
        private readonly Mock<IRepository<Competition>> _mockCompetitionRepository;

        private readonly CompetitionMerger _subject;

        public CompetitionMergerTest()
        {
            _mockCompetitionRepository = new Mock<IRepository<Competition>>();
            
            _subject = new CompetitionMerger(_mockCompetitionRepository.Object);
        }

        [Fact]
        public async Task MergeAsync_Should_Merge_Competitions_Correctly()
        {
            // arrange
            var oldCompetitions = new[]
            {
                new Competition
                {
                    ApiId = 1,
                    Name = "name 1"
                },
                new Competition
                {
                    ApiId = 3,
                    Name = "name 3"
                },
                new Competition
                {
                    ApiId = 4,
                    Name = "name 4"
                }
            };

            var newCompetitions = new[]
            {
                new Competition
                {
                    ApiId = 1,
                    Name = "name 1 updated"
                },
                new Competition
                {
                    ApiId = 2,
                    Name = "name 2"
                },
                new Competition
                {
                    ApiId = 5,
                    Name = "name 5"
                }
            };

            // act
            await _subject.MergeAsync(oldCompetitions, newCompetitions);

            // assert
            _mockCompetitionRepository.Verify(x => x.AddAsync(It.Is<Competition>(c => c.ApiId == 2)), Times.Once);
            _mockCompetitionRepository.Verify(x => x.Remove(It.Is<Competition>(c => c.ApiId == 3)), Times.Once);
            _mockCompetitionRepository.Verify(x => x.Remove(It.Is<Competition>(c => c.ApiId == 4)), Times.Once);
            _mockCompetitionRepository.Verify(x => x.AddAsync(It.Is<Competition>(c => c.ApiId == 5)), Times.Once);

            _mockCompetitionRepository.Verify(x => x.SaveChangesAsync(), Times.Once);

            oldCompetitions[0].Name.Should().Be(newCompetitions[0].Name);
        }
    }
}