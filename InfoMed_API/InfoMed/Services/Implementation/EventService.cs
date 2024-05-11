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
    public class EventService : IEventService
    {
        private readonly InfoMedContext _dbContext;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private readonly IMapper _mapper;

        public EventService(InfoMedContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<EventDto>> GetEvents()
        {
            try
            {
                var events = await _dbContext.Event.ToListAsync();
                return _mapper.Map<List<EventDto>>(events);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null;
            }
        }
    }
}
