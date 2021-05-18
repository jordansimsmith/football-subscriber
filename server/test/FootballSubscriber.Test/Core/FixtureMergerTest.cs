using System.Threading.Tasks;
using FluentAssertions;
using FootballSubscriber.Core.Entities;
using FootballSubscriber.Core.Interfaces;
using FootballSubscriber.Core.Services;
using Moq;
using Xunit;

namespace FootballSubscriber.Test.Core
{
    public class FixtureMergerTest
    {
        private readonly Mock<IFixtureChangeNotificationService> _mockFixtureChangeNotificationService;
        private readonly Mock<IRepository<Fixture>> _mockFixtureRepository;

        private readonly FixtureMerger _subject;

        public FixtureMergerTest()
        {
            _mockFixtureRepository = new Mock<IRepository<Fixture>>();
            _mockFixtureChangeNotificationService = new Mock<IFixtureChangeNotificationService>();

            _subject = new FixtureMerger(_mockFixtureRepository.Object, _mockFixtureChangeNotificationService.Object);
        }

        [Fact]
        public async Task MergerAsync_Should_Merge_Fixtures_Correctly()
        {
            // arrange
            var oldFixtures = new[]
            {
                new Fixture
                {
                    ApiId = 1,
                    Address = "address 1"
                },
                new Fixture
                {
                    ApiId = 3,
                    Address = "address 3"
                },
                new Fixture
                {
                    ApiId = 5,
                    Address = "address 5"
                }
            };

            var newFixtures = new[]
            {
                new Fixture
                {
                    ApiId = 3,
                    Address = "address 3 updated"
                },
                new Fixture
                {
                    ApiId = 4,
                    Address = "address 4"
                },
                new Fixture
                {
                    ApiId = 5,
                    Address = "address 5 updated"
                }
            };

            // act
            await _subject.MergeAsync(oldFixtures, newFixtures);

            // assert
            _mockFixtureRepository.Verify(x => x.Remove(It.Is<Fixture>(f => f.ApiId == 1)), Times.Once);
            _mockFixtureRepository.Verify(x => x.AddAsync(It.Is<Fixture>(f => f.ApiId == 4)), Times.Once);

            _mockFixtureRepository.Verify(x => x.SaveChangesAsync(), Times.Once);

            oldFixtures[1].Address.Should().Be(newFixtures[0].Address);
            oldFixtures[2].Address.Should().Be(newFixtures[2].Address);
        }
    }
}