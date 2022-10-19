using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using FootballSubscriber.Core.Entities;
using FootballSubscriber.Core.Interfaces;
using FootballSubscriber.Core.Mappers;
using FootballSubscriber.Core.Models;
using FootballSubscriber.Core.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FootballSubscriber.Test.Core;

public class RefreshFixtureServiceTest
{
    private readonly Mock<IMerger<Competition>> _mockCompetitionMerger;
    private readonly Mock<IRepository<Competition>> _mockCompetitionRepository;
    private readonly Mock<IFixtureApiService> _mockFixtureApiService;
    private readonly Mock<IMerger<Fixture>> _mockFixtureMerger;
    private readonly Mock<IRepository<Fixture>> _mockFixtureRepository;
    private readonly Mock<ILogger<RefreshFixtureService>> _mockLogger;

    private readonly RefreshFixtureService _subject;

    public RefreshFixtureServiceTest()
    {
        _mockFixtureApiService = new Mock<IFixtureApiService>();
        _mockFixtureRepository = new Mock<IRepository<Fixture>>();
        _mockCompetitionRepository = new Mock<IRepository<Competition>>();
        _mockFixtureMerger = new Mock<IMerger<Fixture>>();
        _mockCompetitionMerger = new Mock<IMerger<Competition>>();
        _mockLogger = new Mock<ILogger<RefreshFixtureService>>();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<FixtureProfile>();
            cfg.AddProfile<CompetitionProfile>();
        });
        var mapper = new Mapper(mapperConfig);

        _subject = new RefreshFixtureService(
            _mockFixtureApiService.Object,
            _mockFixtureRepository.Object,
            _mockCompetitionRepository.Object,
            _mockFixtureMerger.Object,
            _mockCompetitionMerger.Object,
            mapper,
            _mockLogger.Object
        );
    }

    [Fact]
    public async Task RefreshFixturesAsync_Should_Request_Data_Correctly()
    {
        // arrange
        var competitionModels = new[]
        {
            new CompetitionModel
            {
                Id = 123,
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
        _mockFixtureApiService
            .Setup(x => x.GetCompetitionsAsync())
            .ReturnsAsync(competitionModels)
            .Verifiable();

        var organisationModels = new[]
        {
            new OrganisationModel
            {
                Id = "123",
                Name = "My organisation",
                Provider = 0
            }
        };
        _mockFixtureApiService
            .Setup(x => x.GetOrganisationsForCompetitionAsync(It.IsAny<long>()))
            .ReturnsAsync(organisationModels)
            .Verifiable();

        var fixtures = new GetFixturesResponseModel
        {
            FirstFixtureDate = DateTime.Now,
            Fixtures = new[]
            {
                new FixtureModel
                {
                    Id = 1,
                    Address = "Field 1",
                    HomeTeamNameAbbr = "Team 21",
                    AwayTeamNameAbbr = "Team 31"
                },
                new FixtureModel
                {
                    Id = 2,
                    Address = "Field 2",
                    HomeTeamNameAbbr = "Team 41",
                    AwayTeamNameAbbr = "Team 31"
                },
                new FixtureModel
                {
                    Id = 3,
                    Address = "Field 3",
                    HomeTeamNameAbbr = "Team 11",
                    AwayTeamNameAbbr = "Team 51"
                }
            },
            LastResultDate = DateTime.Now,
            RoundInfo = Array.Empty<RoundModel>()
        };
        _mockFixtureApiService
            .Setup(
                x =>
                    x.GetFixturesForCompetitionAsync(It.IsAny<long>(), It.IsAny<IEnumerable<int>>())
            )
            .ReturnsAsync(fixtures)
            .Verifiable();

        _mockFixtureRepository
            .Setup(
                x =>
                    x.FindAsync(
                        It.IsAny<Expression<Func<Fixture, bool>>>(),
                        It.IsAny<Expression<Func<Fixture, object>>>()
                    )
            )
            .ReturnsAsync(Enumerable.Empty<Fixture>())
            .Verifiable();

        var localCompetitions = new[]
        {
            new Competition
            {
                Id = 1,
                ApiId = 123,
                Name = "Competition 123"
            }
        };
        _mockCompetitionRepository
            .Setup(
                x =>
                    x.FindAsync(
                        It.IsAny<Expression<Func<Competition, bool>>>(),
                        It.IsAny<Expression<Func<Competition, object>>>()
                    )
            )
            .ReturnsAsync(localCompetitions)
            .Verifiable();

        // act
        await _subject.RefreshFixturesAsync();

        // assert
        _mockCompetitionMerger.Verify(
            x => x.MergeAsync(It.IsAny<IList<Competition>>(), It.IsAny<IList<Competition>>()),
            Times.Once
        );
        _mockFixtureMerger.Verify(
            x => x.MergeAsync(It.IsAny<IList<Fixture>>(), It.IsAny<IList<Fixture>>()),
            Times.Once
        );

        _mockFixtureApiService.Verify();
        _mockCompetitionRepository.Verify();
        _mockFixtureRepository.Verify();
    }
}
