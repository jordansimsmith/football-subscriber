using System.Threading.Tasks;
using FootballSubscriber.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FootballSubscriber.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TeamsController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamsController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpGet]
    [Route("")]
    public async Task<ActionResult> GetTeamsAsync()
    {
        return Ok(await _teamService.GetTeamsAsync());
    }
}