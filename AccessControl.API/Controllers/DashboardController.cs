using AccessControl.API.Handlers.DashboardHandlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.API.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ISender _sender;   
        public DashboardController(ISender sender) => _sender = sender;

        [HttpGet("info")]
        public async Task<GetSystemInfo.Response> GetSystemInfo() => await _sender.Send(new GetSystemInfo.Request());
    }
}
