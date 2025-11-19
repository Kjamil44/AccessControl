using AccessControl.API.Handlers.ScheduleHandlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.API.Controllers
{
    [Route("api/schedules")]
    [Authorize]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ISender _sender;
        public ScheduleController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<GetSchedules.Response> GetSchedules([FromQuery] GetSchedules.Request request, Guid userId)
        {
            request.UserId = userId;
            return await _sender.Send(request);
        }

        [HttpGet("{scheduleId}")]
        public async Task<GetSchedule.Response> GetSchedule(Guid scheduleId) => await _sender.Send(new GetSchedule.Request { ScheduleId = scheduleId });

        [HttpPost]
        public async Task<AddSchedule.Response> AddSchedule([FromBody] AddSchedule.Request newSchedule) =>
               await _sender.Send(newSchedule);

        [HttpPut("{scheduleId}")]
        public async Task<UpdateSchedule.Response> UpdateSchedule(Guid scheduleId, [FromBody] UpdateSchedule.Request newSchedule)
        {
            newSchedule.ScheduleId = scheduleId;
            return await _sender.Send(newSchedule);
        }

        [HttpDelete("{scheduleId}")]
        public async Task<DeleteSchedule.Response> DeleteSchedule(Guid scheduleId) => await _sender.Send(new DeleteSchedule.Request { ScheduleId = scheduleId });
    }
}
