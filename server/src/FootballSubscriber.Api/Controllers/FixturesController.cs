using System;
using System.Threading.Tasks;
using FootballSubscriber.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FootballSubscriber.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FixturesController: ControllerBase
    {
        private readonly IFixtureService _fixtureService;

        public FixturesController(IFixtureService fixtureService)
        {
            _fixtureService = fixtureService;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> GetFixturesAsync(int competitionId, DateTime fromDate, DateTime toDate)
        {
            return Ok(await _fixtureService.GetFixturesAsync(competitionId, fromDate, toDate));
        }
    }
}