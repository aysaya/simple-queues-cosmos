using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.ServiceBus;
using Contracts;
using RateWebhook.ResourceAccessors;

namespace RateWebhook.Controllers
{
    [Route("api/[controller]")]
    public class ThirdpartyRatesController : Controller
    {
        private readonly ISendMessage<ThirdPartyRate> sender;
        private readonly IQueryRA<ThirdPartyRate> query;
        private readonly ICommandRA<ThirdPartyRate> command;

        public ThirdpartyRatesController(ISendMessage<ThirdPartyRate> sender, 
            IQueryRA<ThirdPartyRate> query, ICommandRA<ThirdPartyRate> command)
        {
            this.sender = sender;
            this.command = command;
            this.query = query;
        }

        [HttpPost]
        public async Task Post([FromBody]ThirdPartyRate value)
        {
            await Task.WhenAll
            (
                command.SaveAsync(value),
                sender.SendAsync(value)
            );            
        }

        [HttpGet]
        public async Task<ThirdPartyRate[]> Get()
        {
            return await query.GetAllAsync();
        }
    }
}
