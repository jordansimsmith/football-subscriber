using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using FootballSubscriber.Core.Interfaces;
using FootballSubscriber.Core.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace FootballSubscriber.Infrastructure.Services;

public class FixtureApiService : IFixtureApiService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public FixtureApiService(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;

        _httpClient.DefaultRequestHeaders.Add("accept", MediaTypeNames.Application.Json);
    }

    public async Task<IEnumerable<CompetitionModel>> GetCompetitionsAsync()
    {
        var payload = new
        {
            compIds = _configuration["FixtureApi:CompetitionIds"],
            seasonId = _configuration["FixtureApi:SeasonId"]
        };
        var stringContent = new StringContent(
            JsonConvert.SerializeObject(payload),
            Encoding.UTF8,
            MediaTypeNames.Application.Json
        );
        var getCompetitionsUri = new Uri(
            $"{_configuration["FixtureApi:BaseAddress"]}/competitionsfromids"
        );
        var response = await _httpClient.PostAsync(getCompetitionsUri, stringContent);

        if (!response.IsSuccessStatusCode)
            throw new SystemException("Could not get competitions");

        var content = await response.Content.ReadAsStringAsync();
        var competitions = JsonConvert.DeserializeObject<IEnumerable<CompetitionModel>>(content);
        return competitions;
    }

    public async Task<IEnumerable<OrganisationModel>> GetOrganisationsForCompetitionAsync(
        long competitionId
    )
    {
        var getOrganisationsUri = new Uri(
            $"{_configuration["FixtureApi:BaseAddress"]}/organisations?ids={competitionId}&season={_configuration["FixtureApi:SeasonId"]}"
        );
        var response = await _httpClient.GetAsync(getOrganisationsUri);

        if (!response.IsSuccessStatusCode)
            throw new SystemException("Could not get organisations for competition");

        var content = await response.Content.ReadAsStringAsync();
        var organisations = JsonConvert.DeserializeObject<IEnumerable<OrganisationModel>>(content);
        return organisations;
    }

    public async Task<GetFixturesResponseModel> GetFixturesForCompetitionAsync(
        long competitionId,
        IEnumerable<int> organisationIds
    )
    {
        var payload = new
        {
            competitionId = competitionId.ToString(),
            from = DateTime.Now.AddYears(-1),
            gradeId = "",
            gradeIds = "",
            orgIds = string.Join(",", organisationIds),
            organisationId = "",
            roundsOn = "False",
            seasonId = _configuration["FixtureApi:SeasonId"],
            to = DateTime.Now.AddYears(1)
        };
        var stringContent = new StringContent(
            JsonConvert.SerializeObject(payload),
            Encoding.UTF8,
            MediaTypeNames.Application.Json
        );

        var getFixturesUri = new Uri(
            $"{_configuration["FixtureApi:BaseAddress"]}/filteredfixtures"
        );
        var response = await _httpClient.PostAsync(getFixturesUri, stringContent);

        if (!response.IsSuccessStatusCode)
            throw new SystemException("Could not get fixtures for competition");

        var content = await response.Content.ReadAsStringAsync();
        var fixtures = JsonConvert.DeserializeObject<GetFixturesResponseModel>(content);

        return fixtures;
    }
}
