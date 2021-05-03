using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using FootballSubscriber.Core.Entities;
using FootballSubscriber.Core.Interfaces;
using FootballSubscriber.Core.Mappers;
using FootballSubscriber.Core.Models;
using FootballSubscriber.Core.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FootballSubscriber.Test.Core
{
    public class RefreshFixtureServiceTest
    {
        private readonly Mock<IFixtureApiService> _mockFixtureApiService;
        private readonly Mock<IRepository<Fixture>> _mockFixtureRepository;
        private readonly Mock<ILogger<RefreshFixtureService>> _mockLogger;

        private readonly RefreshFixtureService _subject;

        private IEnumerable<CompetitionModel> _competitionModels;
        private IEnumerable<OrganisationModel> _organisationModels;

        public RefreshFixtureServiceTest()
        {
            _mockFixtureApiService = new Mock<IFixtureApiService>();
            _mockFixtureRepository = new Mock<IRepository<Fixture>>();
            _mockLogger = new Mock<ILogger<RefreshFixtureService>>();

            var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile<FixtureProfile>(); });
            var mapper = new Mapper(mapperConfig);

            _subject = new RefreshFixtureService(_mockFixtureApiService.Object, _mockFixtureRepository.Object, mapper,
                _mockLogger.Object);

            SetupTestData();
        }

        private void SetupTestData()
        {
            _competitionModels = new[]
            {
                new CompetitionModel
                {
                    Id = "123",
                    Name = "My name",
                    OrganisationId = "456",
                    OrganisationName = "My organisation",
                    Provider = 0,
                    SeasonId = "2021",
                    SeasonName = "2021",
                    SportId = "1",
                    SportName = "Football"
                }
            };
            _mockFixtureApiService.Setup(x => x.GetCompetitionsAsync()).ReturnsAsync(_competitionModels).Verifiable();

            _organisationModels = new[]
            {
                new OrganisationModel
                {
                    Id = "123",
                    Name = "My organisation",
                    Provider = 0
                }
            };
            _mockFixtureApiService.Setup(x => x.GetOrganisationsForCompetitionAsync(It.IsAny<int>()))
                .ReturnsAsync(_organisationModels).Verifiable();
        }

        [Fact]
        public async Task RefreshFixturesAsync_Should_Insert_Fixtures_Correctly()
        {
            // arrange
            var newFixtures = new GetFixturesResponseModel
            {
                FirstFixtureDate = DateTime.Now,
                Fixtures = new[]
                {
                    new FixtureModel
                    {
                        Id = "1",
                        Address = "Field 1"
                    },
                    new FixtureModel
                    {
                        Id = "2",
                        Address = "Field 2"
                    },
                    new FixtureModel
                    {
                        Id = "3",
                        Address = "Field 3"
                    }
                },
                LastResultDate = DateTime.Now,
                RoundInfo = Array.Empty<RoundModel>()
            };

            _mockFixtureApiService
                .Setup(x => x.GetFixturesForCompetitionAsync(It.IsAny<int>(), It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(newFixtures)
                .Verifiable();

            var oldFixtures = Array.Empty<Fixture>();
            _mockFixtureRepository
                .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Fixture, bool>>>(),
                    It.IsAny<Expression<Func<Fixture, object>>>())).ReturnsAsync(oldFixtures).Verifiable();

            // act
            await _subject.RefreshFixturesAsync();

            // assert
            _mockFixtureApiService.Verify();
            _mockFixtureRepository.Verify();

            _mockFixtureRepository.Verify(x => x.AddAsync(It.Is<Fixture>(f => f.ApiId == 1)), Times.Once);
            _mockFixtureRepository.Verify(x => x.AddAsync(It.Is<Fixture>(f => f.ApiId == 2)), Times.Once);
            _mockFixtureRepository.Verify(x => x.AddAsync(It.Is<Fixture>(f => f.ApiId == 3)), Times.Once);
            _mockFixtureRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task RefreshFixturesAsync_Should_Delete_Fixtures_Correctly()
        {
            // arrange
            var newFixtures = new GetFixturesResponseModel
            {
                FirstFixtureDate = DateTime.Now,
                Fixtures = Array.Empty<FixtureModel>(),
                LastResultDate = DateTime.Now,
                RoundInfo = Array.Empty<RoundModel>()
            };

            _mockFixtureApiService
                .Setup(x => x.GetFixturesForCompetitionAsync(It.IsAny<int>(), It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(newFixtures)
                .Verifiable();

            var oldFixtures = new[]
            {
                new Fixture
                {
                    ApiId = 1,
                    Address = "Field 1"
                },
                new Fixture
                {
                    ApiId = 2,
                    Address = "Field 2"
                },
                new Fixture
                {
                    ApiId = 3,
                    Address = "Field 3"
                }
            };

            _mockFixtureRepository
                .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Fixture, bool>>>(),
                    It.IsAny<Expression<Func<Fixture, object>>>())).ReturnsAsync(oldFixtures).Verifiable();

            // act
            await _subject.RefreshFixturesAsync();

            // assert
            _mockFixtureApiService.Verify();
            _mockFixtureRepository.Verify();

            _mockFixtureRepository.Verify(x => x.Remove(It.Is<Fixture>(f => f.ApiId == 1)), Times.Once);
            _mockFixtureRepository.Verify(x => x.Remove(It.Is<Fixture>(f => f.ApiId == 2)), Times.Once);
            _mockFixtureRepository.Verify(x => x.Remove(It.Is<Fixture>(f => f.ApiId == 3)), Times.Once);
            _mockFixtureRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task RefreshFixturesAsync_Should_Merge_Fixtures_Correctly()
        {
            // arrange
            var newFixtures = new GetFixturesResponseModel
            {
                FirstFixtureDate = DateTime.Now,
                Fixtures = new[]
                {
                    new FixtureModel
                    {
                        Id = "1",
                        Address = "Field 1"
                    },
                    new FixtureModel
                    {
                        Id = "2",
                        Address = "Field 4"
                    },
                    new FixtureModel
                    {
                        Id = "5",
                        Address = "Field 5"
                    }
                },
                LastResultDate = DateTime.Now,
                RoundInfo = Array.Empty<RoundModel>()
            };

            _mockFixtureApiService
                .Setup(x => x.GetFixturesForCompetitionAsync(It.IsAny<int>(), It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(newFixtures)
                .Verifiable();

            var oldFixtures = new[]
            {
                new Fixture
                {
                    ApiId = 1,
                    Address = "Field 1"
                },
                new Fixture
                {
                    ApiId = 2,
                    Address = "Field 2"
                },
                new Fixture
                {
                    ApiId = 3,
                    Address = "Field 3"
                }
            };

            _mockFixtureRepository
                .Setup(x => x.FindAsync(It.IsAny<Expression<Func<Fixture, bool>>>(),
                    It.IsAny<Expression<Func<Fixture, object>>>())).ReturnsAsync(oldFixtures).Verifiable();

            // act
            await _subject.RefreshFixturesAsync();

            // assert
            _mockFixtureApiService.Verify();
            _mockFixtureRepository.Verify();

            _mockFixtureRepository.Verify(x => x.Remove(It.Is<Fixture>(f => f.ApiId == 3)), Times.Once);
            _mockFixtureRepository.Verify(x => x.AddAsync(It.Is<Fixture>(f => f.ApiId == 5)), Times.Once);
            _mockFixtureRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
            
            oldFixtures[0].Address.Should().Be("Field 1"); // unchanged
            oldFixtures[1].Address.Should().Be("Field 4"); // changed
        }
    }
}