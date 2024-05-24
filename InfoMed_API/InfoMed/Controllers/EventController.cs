using InfoMed.DTO;
using InfoMed.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InfoMed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

        [HttpGet("GetEventTypes")]
        public async Task<ActionResult<List<EventTypeDto>>> GetEventTypes()
        {
            var eventTypes = await _eventService.GetEventTypes();
            return Ok(eventTypes);
        }

        [HttpPost("AddEvent")]
        public async Task<ActionResult<EventVersionDto>> AddEvent(EventVersionDto _event)
        {
            var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var result = await _eventService.AddEvent(_event, email!);
            if (result != null) return Ok(result);
            return BadRequest("Error occured while creating!");
        }

        [HttpPost("UpdateEvent")]
        public async Task<ActionResult<EventVersionDto>> UpdateEvent(EventVersionDto _event)
        {
            var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var result = await _eventService.UpdateEvent(_event, email!);
            if (result != null) return Ok(result);
            return BadRequest("Error occured while updating!");
        }

        [HttpGet("GetEventById")]

        public async Task<ActionResult<EventVersionDto>> GetEventById(int id)
        {
            var _event = await _eventService.GetEventById(id);
            if (_event != null) return Ok(_event);
            return BadRequest("Error occured while fetching data!");
        }

        [HttpPost("AddSponser")]
        public async Task<ActionResult<bool>> AddSponser(SponsersDto _sponser)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var result = await _eventService.AddSponser(_sponser, userId);
            if (result) return Ok(result);
            return BadRequest("Error occured while creating!");
        }

        [HttpPost("UpdateSponser")]
        public async Task<ActionResult<bool>> UpdateSponser(SponsersDto _sponser)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var result = await _eventService.UpdateSponser(_sponser, userId);
            if (result) return Ok(result);
            return BadRequest("Error occured while updating!");
        }

        [HttpGet("GetSponserById")]
        public async Task<ActionResult<SponsersDto>> GetSponserById(int id)
        {
            var _event = await _eventService.GetSponserById(id);
            if (_event != null) return Ok(_event);
            return BadRequest("Error occured while fetching data!");
        }

        [HttpGet("DeleteSponsor")]
        public async Task<ActionResult<bool>> DeleteSponsor(int id)
        {
            var _event = await _eventService.DeleteSponsor(id);
            if (_event != null) return Ok(_event);
            return BadRequest("Error occured while fetching data!");
        }
        
        [HttpGet("GetSponser")]
        public async Task<ActionResult<List<SponsersDto>>> GetSponser(int eventId)
        {
            var events = await _eventService.GetSponser(eventId);
            return Ok(events);
        }

        [HttpGet("GetSponserTypes")]
        public async Task<ActionResult<List<SponserTypeDto>>> GetSponserTypes()
        {
            var eventTypes = await _eventService.GetSponserTypes();
            return Ok(eventTypes);
        }


        [HttpGet("GetSpeakers")]
        public async Task<ActionResult<List<SpeakersDto>>> GetSpeakers(int eventId)
        {
            var events = await _eventService.GetSpeakers(eventId);
            return Ok(events);
        }

        [HttpGet("GetSpeakerById")]
        public async Task<ActionResult<SpeakersDto>> GetSpeakerById(int id)
        {
            var _sponser = await _eventService.GetSpeakerById(id);
            if (_sponser != null) return Ok(_sponser);
            return BadRequest("Error occured while fetching data!");
        }

        [HttpPost("AddSpeaker")]
        public async Task<ActionResult<bool>> AddSpeaker(SpeakersDto _speaker)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var result = await _eventService.AddSpeaker(_speaker, userId);
            if (result) return Ok(result);
            return BadRequest("Error occured while creating!");
        }

        [HttpPost("UpdateSpeaker")]
        public async Task<ActionResult<bool>> UpdateSpeaker(SpeakersDto _speaker)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var result = await _eventService.UpdateSpeaker(_speaker, userId);
            if (result) return Ok(result);
            return BadRequest("Error occured while updating!");
        }


        [HttpGet("DeleteSpeaker")]
        public async Task<ActionResult<bool>> DeleteSpeaker(int id)
        {
            var _event = await _eventService.DeleteSpeaker(id);
            if (_event != null) return Ok(_event);
            return BadRequest("Error occured while fetching data!");
        }




    }
}
        



    

