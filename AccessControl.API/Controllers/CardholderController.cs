using AccessControl.API.Handlers.CardholderHandlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.API.Controllers
{
    [Route("api/cardholders")]
    [Authorize]
    [ApiController]
    public class CardholderController : ControllerBase
    {
        private readonly ISender _sender;
        public CardholderController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<GetCardholders.Response> GetCardholders([FromQuery] GetCardholders.Request request) => await _sender.Send(request ?? new GetCardholders.Request());

        [HttpGet("{cardholderId}")]
        public async Task<GetCardholder.Response> GetCardholder(Guid cardholderId) => await _sender.Send(new GetCardholder.Request { CardholderId = cardholderId });

        [HttpPost("create")]
        public async Task<AddCardholder.Response> AddCardholder([FromBody] AddCardholder.Request newCardholder) => await _sender.Send(newCardholder);

        [HttpPut("update/{cardholderId}")]
        public async Task<UpdateCardholder.Response> UpdateCardholder(Guid cardholderId, [FromBody] UpdateCardholder.Request newCardholder)
        {
            newCardholder.CardholderId = cardholderId;
            return await _sender.Send(newCardholder);   
        }

        [HttpDelete("delete/{cardholderId}")]
        public async Task<DeleteCardholder.Response> DeleteCardholder(Guid cardholderId) =>
               await _sender.Send(new DeleteCardholder.Request {CardholderId = cardholderId});
    }
}