using InfoMed.DTO;
using InfoMed.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InfoMed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet("GetEvents")]
        public async Task<ActionResult<List<EventVersionDto>>> GetEvents()
        {
            var events = await _eventService.GetEvents();
            return Ok(events);
        }

        [HttpPost("AddEvent")]
        public async Task<ActionResult<bool>> AddEvent(EventVersionDto _event)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var result = await _eventService.AddEvent(_event, userId);
            if (result) return Ok(result);
            return BadRequest("Error occured while creating!");
        }

        [HttpPost("UpdateEvent")]
        public async Task<ActionResult<bool>> UpdateEvent(EventVersionDto _event)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var result = await _eventService.UpdateEvent(_event, userId);
            if (result) return Ok(result);
            return BadRequest("Error occured while updating!");
        }
    }
}
