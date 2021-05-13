using System;
using System.Threading.Tasks;
using FootballSubscriber.Core.Interfaces;
using FootballSubscriber.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballSubscriber.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionsController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpPost("")]
        public async Task<ActionResult> CreateSubscriptionAsync(SubscriptionModel subscriptionModel)
        {
            var subscription =
                await _subscriptionService.CreateSubscriptionAsync(subscriptionModel.TeamId, User.Identity?.Name);
            var location = new Uri($"/subscriptions/{subscription.Id}");
            return Created(location, subscription);
        }

        [HttpGet("")]
        public async Task<ActionResult> GetSubscriptionsAsync()
        {
            var subscriptions =  await _subscriptionService.GetSubscriptionsAsync(User.Identity?.Name);
            return Ok(subscriptions);
        }

        [HttpDelete("{subscriptionId:int}")]
        public async Task<ActionResult> DeleteSubscriptionAsync(int subscriptionId)
        {
            await _subscriptionService.DeleteSubscriptionAsync(subscriptionId, User.Identity?.Name);
            return NoContent();
        }
    }
}