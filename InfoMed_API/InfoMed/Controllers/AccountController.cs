using InfoMed.Data;
using InfoMed.DTO;
using InfoMed.Models;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InfoMed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly InfoMedContext _dbContext;
        public AccountController(UserManager<IdentityUser> userManager, IConfiguration configuration, InfoMedContext dbContext)
        {
            _userManager = userManager;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // Check If user already Exists In [Asp.NetUsers]
                    var userExists = await _userManager.FindByEmailAsync(user.EmailAddress);
                    if (userExists != null)
                        return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User already exists!" });

                    // Create a New User in [Asp.NetUsers]
                    IdentityUser identityUser = new()
                    {
                        Email = user.EmailAddress,
                        UserName = user.UserName,
                        PhoneNumber = user.MobileNumber
                    };

                    User _user = new User()
                    {
                        EmailAddress = user.EmailAddress,
                        UserName = user.UserName,
                        MobileNumber = user.MobileNumber,
                        Role = user.Role,
                        Status = true
                    };

                    await _dbContext.Users.AddAsync(_user);

                    var result = await _userManager.CreateAsync(identityUser, user.Password);

                    if (!result.Succeeded)
                        return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = string.Join(", ", result.Errors.Select(x => x.Description)) });

                    // Assign the new user to the role
                    await _userManager.AddToRoleAsync(identityUser, user.Role);
                    await transaction.CommitAsync();
                    return Ok(new { Status = "Success", Message = "User registered successfully!" });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest("Error occured!");
                }
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserDto loginData)
        {
            try
            {
                var isActiveUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.EmailAddress.Trim() == loginData.EmailAddress.Trim());
                var user = await _userManager.FindByEmailAsync(loginData.EmailAddress);
                if (user != null && isActiveUser?.Status == true && await _userManager.CheckPasswordAsync(user, loginData.Password))
                {
                    // Fetch roles and user-specific information
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var userRole = userRoles.FirstOrDefault();

                    var authClaims = new List<Claim>
                {
                    new(ClaimTypes.Name, user.UserName),
                    new(ClaimTypes.NameIdentifier, user.Id),
                    new(ClaimTypes.Role, userRole!),
                    new(ClaimTypes.Email,user.Email),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));

                    var token = new JwtSecurityToken(
                    _configuration["JwtSettings:ValidIssuer"],
                    _configuration["JwtSettings:ValidAudience"],
                    expires: DateTime.Now.AddDays(30),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                    return Ok(new
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        TokenExpiration = token.ValidTo,
                        UserId = user.Id,
                        UserRole = userRole,
                        UserEmail = user.Email,
                        UserName = user.UserName,
                        LoginTime = DateTime.Now
                    });
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User Not Found!" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);

                if (user == null)
                {
                    return Ok(new { error = true, message = "User does exist!" });
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordDto.ConfirmPassword);

                if (result.Succeeded)
                {
                    return Ok(new { error = false, Message = "Password Reset Sucessfully." });
                }

                return Ok(new { error = true, message = "Password reset failed!" });
            }
            catch (Exception ex)
            {
                throw ex!;
            }
        }
    }
}