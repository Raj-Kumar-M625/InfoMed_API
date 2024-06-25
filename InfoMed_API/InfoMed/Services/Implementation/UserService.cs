using AutoMapper;
using InfoMed.Data;
using InfoMed.DTO;
using InfoMed.Models;
using InfoMed.Services.Interface;
using log4net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace InfoMed.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly InfoMedContext _dbContext;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(InfoMedContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<List<User>> GetUsers()
        {
            try
            {
                var users = await _dbContext.Users.ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }

        public async Task<User> GetUsersbyId(int userId)
        {
            try
            {
                var _user = await _dbContext.Users.FirstOrDefaultAsync(x => x.IdUser == userId);
                return _user!;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }

        public async Task<User> UpdateUser(User user)
        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(user.EmailAddress.Trim());

                if (userExists != null)
                {
                    userExists.UserName = user.UserName;
                    userExists.Email = user.EmailAddress;
                    userExists.PhoneNumber = user.MobileNumber;
                    await _userManager.UpdateAsync(userExists);

                    var userRoles = await _userManager.GetRolesAsync(userExists);
                    foreach (var role in userRoles)
                    {
                        await _userManager.RemoveFromRoleAsync(userExists, role);
                    }
                    await _userManager.AddToRoleAsync(userExists, user.Role);
                }
                else
                {
                    return null!;
                }

                var _user = await _dbContext.Users.FirstOrDefaultAsync(x => x.IdUser == user.IdUser);

                if (_user != null)
                {
                    _user.UserName = user.UserName;
                    _user.EmailAddress = user.EmailAddress;
                    _user.MobileNumber = user.MobileNumber;
                    _user.Status = user.Status;
                    _user.Role = user.Role;

                    _dbContext.Users.Update(_user);
                    await _dbContext.SaveChangesAsync();
                    return _user!;
                }

                return null!;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }
    }
}
