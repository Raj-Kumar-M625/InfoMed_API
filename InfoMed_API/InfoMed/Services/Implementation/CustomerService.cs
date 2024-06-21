using AutoMapper;
using InfoMed.Data;
using InfoMed.DTO;
using InfoMed.Models;
using InfoMed.Services.Interface;
using log4net;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace InfoMed.Services.Implementation
{
    public class CustomerService : ICustomerService
    {
        private readonly InfoMedContext _dbContext;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private readonly IMapper _mapper;

        public CustomerService(InfoMedContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<RegistrationDto> AddRegistrationMembers(RegistrationDto registrationDto)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    if (registrationDto.IdRegistration > 0)
                    {
                        var register = await _dbContext.Registrations.FirstOrDefaultAsync(x => x.IdRegistration == registrationDto.IdRegistration);
                        register.Name = registrationDto.Name;
                        register.EmailID = registrationDto.EmailID;
                        register.MobileNumber = registrationDto.MobileNumber;
                        register.CompanyName = registrationDto.CompanyName;
                        register.CountryName = registrationDto.CountryName;
                        register.ZipCode = registrationDto.ZipCode;
                        register.Address = registrationDto.Address;
                        _dbContext.Registrations.Update(register);

                        var registerMember = await _dbContext.RegistrationMembers.FirstOrDefaultAsync(x => x.IdRegistration == registrationDto.IdRegistration);
                        registerMember.MemberName = registrationDto.Name;
                        registerMember.EmailID = registrationDto.EmailID;
                        registerMember.MobileNumber = registrationDto.MobileNumber;
                        _dbContext.RegistrationMembers.Update(registerMember);

                        await _dbContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return _mapper.Map<RegistrationDto>(register);
                    }
                    
                    Registrations registration = _mapper.Map<Registrations>(registrationDto);
                    registration.RegisteredDate = DateTime.Now;
                    var registrationEntity = await _dbContext.Registrations.AddAsync(registration);
                    await _dbContext.SaveChangesAsync();

                    int registrationId = registrationEntity.Entity.IdRegistration;

                    RegistrationMembers registrationMember = new RegistrationMembers()
                    {
                        IdRegistration = registrationId,
                        MemberName = registrationDto.Name,
                        EmailID = registrationDto.EmailID,
                        MobileNumber = registrationDto.MobileNumber
                    };

                    var entity = await _dbContext.RegistrationMembers.AddAsync(registrationMember);
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    var mapEntity = _mapper.Map<RegistrationDto>(registrationEntity.Entity);
                    return mapEntity;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _log.Error(ex.Message);
                    return null;
                }
            }
        }

        public async Task<RegistrationDto> GetRegistrationMembers(int id, int idVersion)
        {
            try
            {
                var registration = await _dbContext.Registrations.Where(x => x.IdEvent == id ).FirstOrDefaultAsync();
                return _mapper.Map<RegistrationDto>(registration);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }

        public Task<RegistrationDto> UpdateRegistrationMembers(RegistrationDto registrationDto)
        {
            throw new NotImplementedException();
        }

        public async Task<Registrations> GetRegistrationMembersByEmail(string email, int idEvent)
        {
            try
            {
                var registration = await _dbContext.Registrations.Where(x => x.EmailID == email && x.IdEvent == idEvent).FirstOrDefaultAsync();
                return registration;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }
    }
}
