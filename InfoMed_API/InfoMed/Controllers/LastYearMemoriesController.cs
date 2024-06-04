using InfoMed.DTO;
using InfoMed.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InfoMed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LastYearMemoriesController : ControllerBase
    {

        private readonly ILastYearMemoriesService _lastYearMemories;

        public LastYearMemoriesController(ILastYearMemoriesService lastYearMemories)
        {
            _lastYearMemories = lastYearMemories;
        }

        [HttpGet("GetLastYearMemoriesList")]
        public async Task<ActionResult<List<LastYearMemoryDto>>> GetLastYearMemoriesList(int eventId)
        {
            var events = await _lastYearMemories.GetLastYearMemoriesList(eventId);
            return Ok(events);
        }

        [HttpPost("AddLastYearMemories")]
        public async Task<ActionResult<LastYearMemoryDto>> AddLastYearMemories(LastYearMemoryDto lastYeatMemoryDto)
        {
            var LastYearMaster = await _lastYearMemories.AddLastYearMemories(lastYeatMemoryDto);
            if (LastYearMaster != null) return Ok(LastYearMaster);
            return BadRequest("Error occured while fetching data!");
        }
        [HttpPost("UpdateLastYearMemories")]
        public async Task<ActionResult<LastYearMemoryDto>> UpdateLastYearMemories(LastYearMemoryDto lastYeatMemoryDto)
        {
            var LastYearMaster = await _lastYearMemories.UpdateLastYearMemories(lastYeatMemoryDto);
            if (LastYearMaster != null) return Ok(LastYearMaster);
            return BadRequest("Error occured while updating data!");
        }

        [HttpGet("GetLastYearMemoriesById")]
        public async Task<ActionResult<LastYearMemoryDto>> GetLastYearMemoriesById(int idLastYearMemory)
        {
            var LastYearMaster = await _lastYearMemories.GetLastYearMemoriesById(idLastYearMemory);
            if (LastYearMaster != null) return Ok(LastYearMaster);
            return BadRequest("Error occured while fetching data!");
        }


    }
}
