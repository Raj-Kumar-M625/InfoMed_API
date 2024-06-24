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
                        // Update existing registration
                        var register = await _dbContext.Registrations.FirstOrDefaultAsync(x => x.IdRegistration == registrationDto.IdRegistration);
                        if (register != null)
                        {
                            register.Name = registrationDto.Name;
                            register.EmailID = registrationDto.EmailID;
                            register.MobileNumber = registrationDto.MobileNumber;
                            register.CompanyName = registrationDto.CompanyName;
                            register.CountryName = registrationDto.CountryName;
                            register.ZipCode = registrationDto.ZipCode;
                            register.Address = registrationDto.Address;
                            _dbContext.Registrations.Update(register);
                        }

                        var existingMembers = await _dbContext.RegistrationMembers
                            .Where(x => x.IdRegistration == registrationDto.IdRegistration)
                            .ToListAsync();

                        // Update or Insert Members
                        foreach (var memberDto in registrationDto.RegistrationMembers)
                        {
                            var existingMember = existingMembers.FirstOrDefault(x => x.IdRegistrationMember == memberDto.IdRegistrationMember);
                            if (existingMember != null)
                            {
                                // Update existing member
                                existingMember.MemberName = memberDto.MemberName;
                                existingMember.EmailID = memberDto.EmailID;
                                existingMember.MobileNumber = memberDto.MobileNumber;
                                _dbContext.RegistrationMembers.Update(existingMember);
                            }
                            else
                            {
                                // Insert new member
                                var newMember = new RegistrationMembers
                                {
                                    IdRegistration = registrationDto.IdRegistration,
                                    MemberName = memberDto.MemberName,
                                    EmailID = memberDto.EmailID,
                                    MobileNumber = memberDto.MobileNumber
                                };
                                await _dbContext.RegistrationMembers.AddAsync(newMember);
                            }
                        }

                        // Delete members not in the new list
                        foreach (var existingMember in existingMembers)
                        {
                            if (!registrationDto.RegistrationMembers.Any(m => m.IdRegistrationMember == existingMember.IdRegistrationMember))
                            {
                                _dbContext.RegistrationMembers.Remove(existingMember);
                            }
                        }

                        await _dbContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return _mapper.Map<RegistrationDto>(register);
                    }

                    // Create new registration
                    var registration = _mapper.Map<Registrations>(registrationDto);
                    registration.RegisteredDate = DateTime.Now;
                    registration.NoOfPersons = registrationDto.RegistrationMembers?.Count() ?? 0;

                    var registrationEntity = await _dbContext.Registrations.AddAsync(registration);
                    await _dbContext.SaveChangesAsync();

                    if (registrationDto.RegistrationMembers != null)
                    {
                        int registrationId = registrationEntity.Entity.IdRegistration;
                        foreach (var memberDto in registrationDto.RegistrationMembers)
                        {
                            var registrationMember = new RegistrationMembers
                            {
                                IdRegistration = registrationId,
                                MemberName = memberDto.MemberName,
                                EmailID = memberDto.EmailID,
                                MobileNumber = memberDto.MobileNumber
                            };

                            await _dbContext.RegistrationMembers.AddAsync(registrationMember);
                        }
                        await _dbContext.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();
                    return _mapper.Map<RegistrationDto>(registrationEntity.Entity);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _log.Error(ex.Message);
                    return null;
                }
            }
        }


        public async Task<List<RegistrationMemberDto>> GetRegistrationMembers(int id)
        {
            try
            {
                var registration = await _dbContext.RegistrationMembers.Where(x => x.IdRegistration == id ).ToListAsync();
                return _mapper.Map<List<RegistrationMemberDto>>(registration);
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
