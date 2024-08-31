using AccessControl.API.Handlers.SiteHandlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.API.Controllers
{
    [Route("api/sites")]
    [Authorize]
    [ApiController]
    public class SiteController : ControllerBase
    {
        private readonly ISender _sender;
        public SiteController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<GetSites.Response> GetSites(Guid userId) => await _sender.Send(new GetSites.Request { UserId = userId });

        [HttpGet("{siteId}")]
        public async Task<GetSite.Response> GetSite(Guid siteId) => await _sender.Send(new GetSite.Request { SiteId = siteId });

        [HttpPost("create")]
        public async Task<AddSite.Response> CreateSite(Guid userId, [FromBody] AddSite.Request newSite)
        {
            newSite.UserId = userId;
            return await _sender.Send(newSite);
        }

        [HttpPut("update/{siteId}")]
        public async Task<UpdateSite.Response> UpdateSite(Guid siteId, [FromBody] UpdateSite.Request newSite)
        {
            newSite.SiteId = siteId;
            return await _sender.Send(newSite);
        }

        [HttpDelete("delete/{siteId}")]
        public async Task<DeleteSite.Response> DeleteSite(Guid siteId) => await _sender.Send(new DeleteSite.Request { SiteId = siteId });
    }
}
