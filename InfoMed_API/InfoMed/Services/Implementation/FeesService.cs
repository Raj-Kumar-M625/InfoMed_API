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
    public class FeesService : IFeesService
    {
        private readonly InfoMedContext _dbContext;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private readonly IMapper _mapper;

        public FeesService(InfoMedContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<List<ConferenceFeeDto>> GetConferenceFeesList(int id, int idVersion)
        {
            try
            {
                var events = await _dbContext.ConferenceFees.Where(x=>x.IdEvent == id && x.IdEventVersion == idVersion).OrderBy(x=>x.OrderNumber).Where(x=>x.IsActive==true).ToListAsync();
                return _mapper.Map<List<ConferenceFeeDto>>(events);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }

        public async Task<ConferenceFeeDto> AddFees(ConferenceFeeDto feesMasterDto)
        {
            try
            {
                ConferenceFees scheduleMaster = _mapper.Map<ConferenceFees>(feesMasterDto);
                //var _event = await _dbContext.EventVersions.FirstOrDefaultAsync(x => x.IdEvent == feesMasterDto.IdEventVersion);
                //if (_event != null) scheduleMaster.IdEvent = _event.IdEvent;
                var feesMasterEntity = await _dbContext.ConferenceFees.AddAsync(scheduleMaster);
                await _dbContext.SaveChangesAsync();
                return _mapper.Map<ConferenceFeeDto>(feesMasterEntity.Entity);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }

        public async Task<ConferenceFeeDto> GetFeesById(int GetFeesById)
        {
            try
            {
                var feesMaster = await _dbContext.ConferenceFees
                                                      .FirstOrDefaultAsync(x => x.IdConferenceFee == GetFeesById);
                return _mapper.Map<ConferenceFeeDto>(feesMaster);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }

        public async Task<ConferenceFeeDto> UpdateFees(ConferenceFeeDto feesMasterDto)
        {
            try
            {
                var feesMaster = await _dbContext.ConferenceFees
                                                     .FirstOrDefaultAsync(x => x.IdConferenceFee == feesMasterDto.IdConferenceFee);
                if (feesMaster != null)
                {
                    feesMaster.FeeName = feesMasterDto.FeeName;
                    feesMaster.MinimumPeopleCount = feesMasterDto.MinimumPeopleCount;
                    feesMaster.OrderNumber = feesMasterDto.OrderNumber;
                    feesMaster.DayCount = feesMasterDto.DayCount;
                    feesMaster.Amount = feesMasterDto.Amount;
                    feesMaster.ApplicableStartDate = feesMasterDto.ApplicableStartDate;
                    feesMaster.ApplicableEndDate = feesMasterDto.ApplicableEndDate;
                    feesMaster.FeeDetailText = feesMasterDto.FeeDetailText;
                    feesMaster.IsActive = feesMasterDto.IsActive;
                    var scheduleMasterEntity = _dbContext.ConferenceFees.Update(feesMaster);
                    await _dbContext.SaveChangesAsync();
                    return _mapper.Map<ConferenceFeeDto>(scheduleMasterEntity.Entity);
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
