using InfoMed.Models;
using InfoMed.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InfoMed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ICommonService _commonService;

        public CommonController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        [HttpGet("GetCountries")]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _commonService.GetCountries();
            return Ok(countries);
        }
    }
}
