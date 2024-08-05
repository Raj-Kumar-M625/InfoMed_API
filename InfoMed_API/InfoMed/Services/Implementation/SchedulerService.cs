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
    public class SchedulerService : ISchedulerService
    {
        private readonly InfoMedContext _dbContext;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private readonly IMapper _mapper;

        public SchedulerService(InfoMedContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<ScheduleMasterDto>> GetSchedulesMaster(int id, int idVersion)
        {
            try
            {
                var scheduleMaster = await _dbContext.ScheduleMaster.Where(x => x.IdEvent == id && x.IdEventVersion == idVersion && x.IsActive == true)
                                                                    .ToListAsync();
                return _mapper.Map<List<ScheduleMasterDto>>(scheduleMaster);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }

        public async Task<List<ScheduleDetailsDto>> GetSchedulesDetails(int IdScheduleMaster)
        {
            try
            {
                var scheduleDetails = await _dbContext.ScheduleDetails.Where(x => x.IdScheduleMaster == IdScheduleMaster && x.IsActive == true)
                                                                      .OrderBy(x => x.StartTime.TimeOfDay)
                                                                      .ToListAsync();
                return _mapper.Map<List<ScheduleDetailsDto>>(scheduleDetails);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null;
            }
        }

        public async Task<ScheduleMasterDto> AddScheduleMaster(ScheduleMasterDto ScheduleMasterDto)
        {
            try
            {
                ScheduleMaster scheduleMaster = _mapper.Map<ScheduleMaster>(ScheduleMasterDto);
                //var _event = await _dbContext.EventVersions.FirstOrDefaultAsync(x => x.IdEventVersion == ScheduleMasterDto.IdEventVersion);
                //if (_event != null) scheduleMaster.IdEvent = _event.IdEvent;
                var ScheduleMasterEntity = await _dbContext.ScheduleMaster.AddAsync(scheduleMaster);
                await _dbContext.SaveChangesAsync();
                return _mapper.Map<ScheduleMasterDto>(ScheduleMasterEntity.Entity);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }

        public async Task<ScheduleDetailsDto> AddScheduleDetails(ScheduleDetailsDto ScheduleDetailsDto)
        {
            try
            {
                ScheduleDetails scheduleDetails = _mapper.Map<ScheduleDetails>(ScheduleDetailsDto);
                var ScheduleDetailsEntity = await _dbContext.ScheduleDetails.AddAsync(scheduleDetails);
                await _dbContext.SaveChangesAsync();
                return _mapper.Map<ScheduleDetailsDto>(ScheduleDetailsEntity.Entity);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }

        public async Task<ScheduleDetailsDto> GetScheduleDetailById(int idScheduleDetail)
        {
            try
            {
                var schedule = await _dbContext.ScheduleDetails
                                               .FirstOrDefaultAsync(x => x.IdScheduleDetail == idScheduleDetail);
                return _mapper.Map<ScheduleDetailsDto>(schedule);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }

        public async Task<ScheduleMasterDto> UpdateScheduleMaster(ScheduleMasterDto scheduleMasterDto)
        {
            try
            {
                var scheduleMaster = await _dbContext.ScheduleMaster
                                                     .FirstOrDefaultAsync(x => x.IdScheduleMaster == scheduleMasterDto.IdScheduleMaster);
                if (scheduleMaster != null)
                {
                    scheduleMaster.ScheduleDate = scheduleMasterDto.ScheduleDate;
                    scheduleMaster.DayScheduleName = scheduleMasterDto.DayScheduleName;
                    scheduleMaster.DayScheduleDetailsText = scheduleMasterDto.DayScheduleDetailsText;
                    scheduleMaster.IsActive = scheduleMasterDto.IsActive;
                    var scheduleMasterEntity = _dbContext.ScheduleMaster.Update(scheduleMaster);
                    await _dbContext.SaveChangesAsync();
                    return _mapper.Map<ScheduleMasterDto>(scheduleMasterEntity.Entity);
                }
                return null!;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }

        public async Task<ScheduleDetailsDto> UpdateScheduleDetails(ScheduleDetailsDto scheduleDetailsDto)
        {
            try
            {
                var scheduleDetails = await _dbContext.ScheduleDetails
                                                     .FirstOrDefaultAsync(x => x.IdScheduleDetail == scheduleDetailsDto.IdScheduleDetail);
                if (scheduleDetails != null)
                {
                    scheduleDetails.StartTime = scheduleDetailsDto.StartTime;
                    scheduleDetails.EndTime = scheduleDetailsDto.EndTime;
                    scheduleDetails.Topic = scheduleDetailsDto.Topic;
                    scheduleDetails.TopicName = scheduleDetailsDto.TopicName;
                    scheduleDetails.IsActive = scheduleDetailsDto.IsActive;
                    var scheduleDetailsEntity = _dbContext.ScheduleDetails.Update(scheduleDetails);
                    await _dbContext.SaveChangesAsync();
                    return _mapper.Map<ScheduleDetailsDto>(scheduleDetailsEntity.Entity);
                }
                return null!;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }

        public async Task<ScheduleMasterDto> GetScheduleMasterById(int idScheduleMaster)
        {
            try
            {
                var scheduleMaster = await _dbContext.ScheduleMaster
                                                      .FirstOrDefaultAsync(x => x.IdScheduleMaster == idScheduleMaster);
                return _mapper.Map<ScheduleMasterDto>(scheduleMaster);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }
    }
}

