using System.Threading.Tasks;
using FootballSubscriber.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FootballSubscriber.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CompetitionsController : ControllerBase
{
    private readonly ICompetitionService _competitionService;

    public CompetitionsController(ICompetitionService competitionService)
    {
        _competitionService = competitionService;
    }

    [HttpGet]
    [Route("")]
    public async Task<ActionResult> GetCompetitionsAsync()
    {
        return Ok(await _competitionService.GetCompetitionsAsync());
    }
}
