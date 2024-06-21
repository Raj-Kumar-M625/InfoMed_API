using InfoMed.DTO;
using InfoMed.Models;

namespace InfoMed.Services.Interface
{
    public interface ICustomerService
    {
        public Task<RegistrationDto> GetRegistrationMembers(int id, int idVersion);
        public Task<Registrations> GetRegistrationMembersByEmail(string email,int idEvent);
        public Task<RegistrationDto> AddRegistrationMembers(RegistrationDto registrationDto);        
        public Task<RegistrationDto> UpdateRegistrationMembers(RegistrationDto registrationDto);
    }
}
