using AccessControl.API.Handlers.LiveEventHandlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.API.Controllers
{
    [Route("api/live-events")]
    [Authorize]
    [ApiController]
    public class LiveEventController : ControllerBase
    {
        private readonly ISender _sender;

        public LiveEventController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<GetLiveEvents.Response> GetLiveEvents() 
            => await _sender.Send(new GetLiveEvents.Request());
    }
}