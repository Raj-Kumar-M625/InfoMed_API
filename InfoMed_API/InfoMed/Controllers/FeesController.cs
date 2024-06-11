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
    public class FeesController : ControllerBase
    {

        private readonly IFeesService _feesService;

        public FeesController(IFeesService feeService)
        {
            _feesService = feeService;
        }

        [HttpGet("GetConferenceFeesList")]
        public async Task<ActionResult<List<ConferenceFeeDto>>> GetConferenceFeesList(int id, int idVersion) 
        {
            var events = await _feesService.GetConferenceFeesList(id,idVersion);
            return Ok(events);
        }
        
        [HttpPost("AddFees")]
        public async Task<ActionResult<ConferenceFeeDto>> AddFees(ConferenceFeeDto feesMasterDto)
        {
            var feesMaster = await _feesService.AddFees(feesMasterDto);
            if (feesMaster != null) return Ok(feesMaster);
            return BadRequest("Error occured while fetching data!");
        }
        [HttpPost("UpdateFees")]
        public async Task<ActionResult<ConferenceFeeDto>> UpdateFees(ConferenceFeeDto feesMasterDto)
        {
            var feesMaster = await _feesService.UpdateFees(feesMasterDto);
            if (feesMaster != null) return Ok(feesMaster);
            return BadRequest("Error occured while updating data!");
        }

        [HttpGet("GetFeesById")]
        public async Task<ActionResult<ConferenceFeeDto>> GetFeesById(int idFeesMaster)
        {
            var feesMaster = await _feesService.GetFeesById(idFeesMaster);
            if (feesMaster != null) return Ok(feesMaster);
            return BadRequest("Error occured while fetching data!");
        }

    }
}
