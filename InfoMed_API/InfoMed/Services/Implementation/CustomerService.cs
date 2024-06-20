using AutoMapper;
using InfoMed.Data;
using InfoMed.DTO;
using InfoMed.Models;
using InfoMed.Services.Interface;
using log4net;
using Microsoft.EntityFrameworkCore;
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
                    Registration registration = _mapper.Map<Registration>(registrationDto);
                    
                    var registrationEntity = await _dbContext.Registration.AddAsync(registration);
                    await _dbContext.SaveChangesAsync();
                    int registrationId = registrationEntity.Entity.IdRegistration;
                    registrationDto.RegistrationMember.IdRegistration = registrationId;
                    var registrationMemberDetails = _mapper.Map<RegistrationMember>(registrationDto.RegistrationMember);;
                    var entity = await _dbContext.RegistrationMember.AddAsync(registrationMemberDetails);
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
                var registration = await _dbContext.Registration.Where(x => x.IdEvent == id ).Include(o => o.RegistrationMember).FirstOrDefaultAsync();
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
    }
}
