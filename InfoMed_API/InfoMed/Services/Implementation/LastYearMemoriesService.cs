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
    public class LastYearMemoriesService : ILastYearMemoriesService
    {
        private readonly InfoMedContext _dbContext;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private readonly IMapper _mapper;

        public LastYearMemoriesService(InfoMedContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<List<LastYearMemoryDto>> GetLastYearMemoriesList(int id, int idVersion)
        {
            try
            {
                var events = await _dbContext.LastYearMemories.Include(o => o.LastYearMemoryDetail).Where(x => x.IdEvent == id && x.IdEventVersion == idVersion).OrderBy(x => x.LastYearMemoryDetail.OrderNumber).Where(x => x.Status == true).ToListAsync();
                return _mapper.Map<List<LastYearMemoryDto>>(events);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }

        public async Task<LastYearMemoryDto> AddLastYearMemories(LastYearMemoryDto lastYeatMemoryDto)
        {            

                using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                    LastYearMemory lastYearMemory = _mapper.Map<LastYearMemory>(lastYeatMemoryDto);
                    //var _event = await _dbContext.EventVersions.FirstOrDefaultAsync(x => x.IdEvent == lastYeatMemoryDto.IdEventVersion);
                    //if (_event != null) lastYearMemory.IdEvent = _event.IdEvent;
                    lastYearMemory.LastYearMemoryDetail = null;
                    var lastYearMemoryEntity = await _dbContext.LastYearMemories.AddAsync(lastYearMemory);
                    await _dbContext.SaveChangesAsync();
                    int lastYearMemoryId = lastYearMemoryEntity.Entity.IdLastYearMemory;
                    lastYeatMemoryDto.LastYearMemoryDetail.IdLastYearMemory = lastYearMemoryId;
                    var newEvent = _mapper.Map<LastYearMemoryDetail>(lastYeatMemoryDto.LastYearMemoryDetail);

                    var lastYear = await _dbContext.LastYearMemoryDetails
                .Where(tc => tc.OrderNumber >= newEvent.OrderNumber && tc.IdLastYearMemory == newEvent.IdLastYearMemory)
                .OrderBy(tc => tc.OrderNumber)
                .ToListAsync();
                    if (lastYear.Any())
                    {
                        foreach (var content in lastYear)
                        {
                            content.OrderNumber++;
                        }

                        _dbContext.LastYearMemoryDetails.UpdateRange(lastYear);
                    }

                    newEvent.IdLastYearMemory = lastYearMemoryId;
                        var entity = await _dbContext.LastYearMemoryDetails.AddAsync(newEvent);
                        await _dbContext.SaveChangesAsync();

                        await transaction.CommitAsync();

                        var mapEntity = _mapper.Map<LastYearMemoryDto>(lastYearMemoryEntity.Entity);
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

        public async Task<LastYearMemoryDto> UpdateLastYearMemories(LastYearMemoryDto LastYearMemoryDto)
        {
            try
            {
                var lastYearMemory = await _dbContext.LastYearMemories.Include(o => o.LastYearMemoryDetail).Where(x => x.IdLastYearMemory == LastYearMemoryDto.IdLastYearMemory).FirstOrDefaultAsync();
                if (lastYearMemory != null)
                {
                    lastYearMemory.LastYearMemoryHeader = LastYearMemoryDto.LastYearMemoryHeader;
                    lastYearMemory.LastYearMemoryText = LastYearMemoryDto.LastYearMemoryText;
                    lastYearMemory.Status = LastYearMemoryDto.Status;
                    var lastYearMemoryEntity = _dbContext.LastYearMemories.Update(lastYearMemory);
                    if(LastYearMemoryDto.Status != false)
                    {
                        var lastYear = await _dbContext.LastYearMemoryDetails
              .Where(tc => tc.OrderNumber >= LastYearMemoryDto.LastYearMemoryDetail.OrderNumber && tc.IdLastYearMemory == LastYearMemoryDto.LastYearMemoryDetail.IdLastYearMemory)
              .OrderBy(tc => tc.OrderNumber)
              .ToListAsync();
                        if (lastYear.Any())
                        {
                            foreach (var content in lastYear)
                            {
                                content.OrderNumber++;
                            }

                            _dbContext.LastYearMemoryDetails.UpdateRange(lastYear);
                        }
                    }
                   

                    lastYearMemory.LastYearMemoryDetail.OrderNumber = LastYearMemoryDto.LastYearMemoryDetail.OrderNumber;
                    lastYearMemory.LastYearMemoryDetail.MediaShortDesc = LastYearMemoryDto.LastYearMemoryDetail.MediaShortDesc;                   
                    lastYearMemory.LastYearMemoryDetail.MediaType = LastYearMemoryDto.LastYearMemoryDetail.MediaType;                   
                    var lastYearMemoryDetailsEntity = _dbContext.LastYearMemoryDetails.Update(lastYearMemory.LastYearMemoryDetail);
                    await _dbContext.SaveChangesAsync();
                    return _mapper.Map<LastYearMemoryDto>(lastYearMemoryEntity.Entity);
                }
                return null!;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }

        public async Task<LastYearMemoryDto> GetLastYearMemoriesById(int idLastYearMemory)
        {
            try
            {
                var lastYearMemory =  await _dbContext.LastYearMemories.Include(o => o.LastYearMemoryDetail).Where(x => x.IdLastYearMemory == idLastYearMemory).FirstOrDefaultAsync();
                return _mapper.Map<LastYearMemoryDto>(lastYearMemory);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }
    }
}
