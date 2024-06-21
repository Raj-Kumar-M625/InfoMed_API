using InfoMed.DTO;
using InfoMed.Models;
using InfoMed.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace InfoMed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("GetRegistrationMembers")]
        public async Task<ActionResult<RegistrationDto>> GetRegistrationMembers(int id, int idVersion) 
        {
            var register = await _customerService.GetRegistrationMembers(id, idVersion);
            return Ok(register);
        }

        [HttpPost("AddRegistrationMembers")]
        public async Task<ActionResult<RegistrationDto>> AddRegistrationMembers(RegistrationDto registrationDto)
        {
            var register = await _customerService.AddRegistrationMembers(registrationDto);
            if (register != null) return Ok(register);
            return BadRequest("Error occured while fetching data!");
        }

        [HttpPost("UpdateRegistrationMembers")]
        public async Task<ActionResult<RegistrationDto>> UpdateRegistrationMembers(RegistrationDto registrationDto)
        {
            var register = await _customerService.UpdateRegistrationMembers(registrationDto);
            if (register != null) return Ok(register);
            return BadRequest("Error occured while updating data!");
        }

        [HttpGet("GetRegistrationMembersByEmail")]
        public async Task<ActionResult<Registrations>> GetRegistrationMembersByEmail(string email, int idEvent)
        {
            var register = await _customerService.GetRegistrationMembersByEmail(email,idEvent);
            return Ok(register);
        }
    }
}
