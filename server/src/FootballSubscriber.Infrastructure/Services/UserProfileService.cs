using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Auth0.ManagementApi;
using FootballSubscriber.Core.Interfaces;
using FootballSubscriber.Core.Models;
using FootballSubscriber.Infrastructure.Models;
using Microsoft.Extensions.Configuration;

namespace FootballSubscriber.Infrastructure.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly IManagementConnection _managementConnection;

        public UserProfileService(HttpClient httpClient, IConfiguration configuration, IManagementConnection managementConnection)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _managementConnection = managementConnection;
        }

        public async Task<UserProfile> GetUserProfileAsync(string userId)
        {
            var accessToken = await GetAccessTokenAsync();
            var baseUri = new Uri(_configuration["Auth0:ManagementAudience"]);

            using var managementClient = new ManagementApiClient(accessToken.AccessToken, baseUri, _managementConnection);

            var user = await managementClient.Users.GetAsync(userId);

            return new UserProfile
            {
                Name = user.FirstName,
                Email = user.Email
            };
        }

        private async Task<AccessTokenModel> GetAccessTokenAsync()
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {
                    "client_id", _configuration["Auth0:ManagementClientId"]
                },
                {
                    "client_secret", _configuration["Auth0:ManagementClientSecret"]
                },
                {
                    "grant_type", "client_credentials"
                },
                {
                    "audience", _configuration["Auth0:ManagementAudience"]
                }
            });
            var getTokenUrl = $"{_configuration["Auth0:ManagementBaseAddress"]}/oauth/token";

            var response = await _httpClient.PostAsync(getTokenUrl, content);

            var responseStream = await response.Content.ReadAsStreamAsync();
            var accessToken = await JsonSerializer.DeserializeAsync<AccessTokenModel>(responseStream);

            return accessToken;
        }
    }
}