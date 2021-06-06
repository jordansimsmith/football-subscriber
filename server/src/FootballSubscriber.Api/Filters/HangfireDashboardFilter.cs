using System;
using System.Net.Http.Headers;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;

namespace FootballSubscriber.Api.Filters
{
    public class HangfireDashboardFilter : IDashboardAuthorizationFilter
    {
        private readonly string _hangfireUser;
        private readonly string _hangfirePassword;

        public HangfireDashboardFilter(string hangfireUser, string hangfirePassword)
        {
            if (string.IsNullOrWhiteSpace(hangfireUser) || string.IsNullOrWhiteSpace(hangfirePassword))
            {
                throw new ArgumentException("Hangfire user or password cannot be empty");
            }

            _hangfireUser = hangfireUser;
            _hangfirePassword = hangfirePassword;
        }

        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            try
            {
                var header = httpContext.Request.Headers["Authorization"];

                if (string.IsNullOrWhiteSpace(header))
                {
                    return SetChallengeResponse(httpContext);
                }

                var authValues = AuthenticationHeaderValue.Parse(header);

                var isBasicAuthentication =
                    "Basic".Equals(authValues.Scheme, StringComparison.InvariantCultureIgnoreCase);
                if (!isBasicAuthentication)
                {
                    return SetChallengeResponse(httpContext);
                }

                var parameter = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authValues.Parameter));
                var parts = parameter.Split(':');

                var username = parts[0];
                var password = parts[1];

                if (username != _hangfireUser || password != _hangfirePassword)
                {
                    return SetChallengeResponse(httpContext);
                }

                // success
                return true;
            }
            catch
            {
                return SetChallengeResponse(httpContext);
            }
        }

        private bool SetChallengeResponse(HttpContext httpContext)
        {
            httpContext.Response.Headers.Append("WWW-Authenticate", "Basic");

            return false;
        }
    }
}