using InfoMed.DTO;

namespace InfoMed.Services.Interface
{
    public interface ICustomerService
    {
        public Task<RegistrationDto> GetRegistrationMembers(int id, int idVersion);
        public Task<RegistrationDto> AddRegistrationMembers(RegistrationDto registrationDto);        
        public Task<RegistrationDto> UpdateRegistrationMembers(RegistrationDto registrationDto);
    }
}
