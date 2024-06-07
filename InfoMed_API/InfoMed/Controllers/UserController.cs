using InfoMed.Models;
using InfoMed.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InfoMed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(IUserService userService, RoleManager<IdentityRole> roleManager)
        {
            _userService = userService;
            _roleManager = roleManager;
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
             var users = await _userService.GetUsers();
            return Ok(users);
        }

        [HttpGet("GetUsersbyId")]
        public async Task<IActionResult> GetUsersbyId(int userId)
        {
            var user = await _userService.GetUsersbyId(userId);
            return Ok(user);
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser(User user)
        {
            var _user = await _userService.UpdateUser(user);
            return Ok(user);
        }


        [HttpGet("GetUserRoles")]
        public async Task<IActionResult> GetUserRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(roles);
        }
    }
}
