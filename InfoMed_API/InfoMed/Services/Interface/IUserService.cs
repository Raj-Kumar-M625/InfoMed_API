using InfoMed.DTO;
using InfoMed.Models;

namespace InfoMed.Services.Interface
{
    public interface IUserService
    {
        public Task<List<User>> GetUsers();
        public Task<User> GetUsersbyId(int userId);
        public Task<User> UpdateUser(User user);
    }
}
