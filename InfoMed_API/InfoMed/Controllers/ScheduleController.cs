using InfoMed.DTO;
using InfoMed.Models;
using InfoMed.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfoMed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ScheduleController : ControllerBase
    {
        private readonly ISchedulerService _schedulerService;

        public ScheduleController(ISchedulerService schedulerService)
        {
            _schedulerService = schedulerService;
        }

        [HttpGet("GetSchedulesMaster")]
        public async Task<ActionResult<List<ScheduleMasterDto>>> GetSchedulesMaster(int id, int idVersion)
        {
            var schedulesMaster = await _schedulerService.GetSchedulesMaster(id,idVersion);
            if(schedulesMaster != null) return Ok(schedulesMaster);
            return BadRequest("Error occured while fetching data!");
        }

        [HttpGet("GetSchedulesDetails")]
        public async Task<ActionResult<List<ScheduleDetailsDto>>> GetSchedulesDetails(int idScheduleMaster)
        {
            var scheduleDetails = await _schedulerService.GetSchedulesDetails(idScheduleMaster);
            if(scheduleDetails != null) return Ok(scheduleDetails);
            return BadRequest("Error occured while fetching data!");
        }

        [HttpPost("AddScheduleMaster")]
        public async Task<ActionResult<ScheduleMasterDto>> AddScheduleMaster(ScheduleMasterDto scheduleMasterDto)
        {
            var schedulesMaster = await _schedulerService.AddScheduleMaster(scheduleMasterDto);
            if (schedulesMaster != null) return Ok(schedulesMaster);
            return BadRequest("Error occured while fetching data!");
        }

        [HttpPost("AddScheduleDetails")]
        public async Task<ActionResult<ScheduleDetailsDto>> AddScheduleDetails(ScheduleDetailsDto scheduleDetailsDto)
        {
            var scheduleDetails = await _schedulerService.AddScheduleDetails(scheduleDetailsDto);
            if (scheduleDetails != null) return Ok(scheduleDetails);
            return BadRequest("Error occured while fetching data!");
        }

        [HttpPost("UpdateScheduleMaster")]
        public async Task<ActionResult<ScheduleMasterDto>> UpdateScheduleMaster(ScheduleMasterDto scheduleMasterDto)
        {
            var schedulesMaster = await _schedulerService.UpdateScheduleMaster(scheduleMasterDto);
            if (schedulesMaster != null) return Ok(schedulesMaster);
            return BadRequest("Error occured while updating data!");
        }

        [HttpPost("UpdateScheduleDetails")]
        public async Task<ActionResult<ScheduleDetailsDto>> UpdateScheduleDetails(ScheduleDetailsDto scheduleDetailsDto)
        {
            var scheduleDetails = await _schedulerService.UpdateScheduleDetails(scheduleDetailsDto);
            if (scheduleDetails != null) return Ok(scheduleDetails);
            return BadRequest("Error occured while updating data!");
        }

        [HttpGet("GetScheduleDetailById")]
        public async Task<ActionResult<ScheduleDetailsDto>> GetScheduleDetailById(int idScheduleDetails)
        {
            var scheduleDetails = await _schedulerService.GetScheduleDetailById(idScheduleDetails);
            if (scheduleDetails != null) return Ok(scheduleDetails);
            return BadRequest("Error occured while fetching data!");
        }

        [HttpGet("GetScheduleMasterById")]
        public async Task<ActionResult<ScheduleMasterDto>> GetScheduleMasterById(int idScheduleMaster)
        {
            var scheduleMaster = await _schedulerService.GetScheduleMasterById(idScheduleMaster);
            if (scheduleMaster != null) return Ok(scheduleMaster);
            return BadRequest("Error occured while fetching data!");
        }
    }
}
