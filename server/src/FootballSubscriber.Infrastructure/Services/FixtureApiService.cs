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

namespace FootballSubscriber.Infrastructure.Services
{
    public class FixtureApiService : IFixtureApiService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public FixtureApiService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri(_configuration["FixtureApi:BaseAddress"]);
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
                MediaTypeNames.Application.Json);
            var response = await _httpClient.PostAsync("competitionsfromids", stringContent);

            if (!response.IsSuccessStatusCode) throw new SystemException("Could not get competitions");

            var content = await response.Content.ReadAsStringAsync();
            var competitions = JsonConvert.DeserializeObject<IEnumerable<CompetitionModel>>(content);
            return competitions;
        }
    }
}