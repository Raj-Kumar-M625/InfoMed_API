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

        public async Task<List<EventVersionDto>> GetEvents()
        {
            try
            {
                var events = await _dbContext.EventVersions.ToListAsync();
                return _mapper.Map<List<EventVersionDto>>(events);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }

        public async Task<EventVersionDto> GetEventById(int id)
        {
            try
            {
                var _event = await _dbContext.EventVersions.FirstOrDefaultAsync(x => x.IdEventVersion == id);
                return _mapper.Map<EventVersionDto>(_event);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }

        public async Task<EventVersionDto> AddEvent(EventVersionDto _event, string email)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.EmailAddress == email);
                    EventsMaster eventsMaster = new EventsMaster()
                    {
                        EventMasterName = _event.EventName,
                        CreatedBy = user!.IdUser,
                        CreatedDate = DateTime.Now,
                    };

                    var result = await _dbContext.EventsMaster.AddAsync(eventsMaster);
                    await _dbContext.SaveChangesAsync();

                    var prevEvent = await _dbContext.EventVersions.OrderBy(x => x.IdEventVersion).LastOrDefaultAsync();

                    var newEvent = _mapper.Map<EventVersions>(_event);
                    newEvent.IdEvent = result.Entity.IdEvent;
                    newEvent.IdVersion = prevEvent != null ? prevEvent.IdVersion + 1 : 1;

                    var entity = await _dbContext.EventVersions.AddAsync(newEvent);
                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();

                    var mapEntity = _mapper.Map<EventVersionDto>(entity.Entity);
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

        public async Task<EventVersionDto> UpdateEvent(EventVersionDto _event, string email)
        {
            try
            {
                var dbObject = await _dbContext.EventVersions.FirstOrDefaultAsync(x => x.IdEventVersion == _event.IdEventVersion);
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.EmailAddress == email);
                if (dbObject != null)
                {
                    dbObject.IdEvent = _event.IdEvent;
                    dbObject.IdVersion = _event.IdVersion;
                    dbObject.VersionStatus = _event.VersionStatus;
                    dbObject.EventWebPageName = _event.EventWebPageName;
                    dbObject.EventName = _event.EventName;
                    dbObject.EventType = _event.EventType;
                    dbObject.VenueName = _event.VenueName;
                    dbObject.VenueAddress = _event.VenueAddress;
                    dbObject.VenueLatLong = _event.VenueLatLong;
                    dbObject.EventHomeContent = _event.EventHomeContent;
                    dbObject.FooterText = _event.FooterText;
                    dbObject.FacebookLink = _event.FacebookLink;
                    dbObject.TwitterLink = _event.TwitterLink;
                    dbObject.LinkedInLink = _event.LinkedInLink;
                    dbObject.CopyrightText = _event.CopyrightText;
                    dbObject.ShowHurryUpContent = _event.ShowHurryUpContent;
                    dbObject.ApprovedBy = _event.ApprovedBy;
                    dbObject.ApprovedDate = _event.ApprovedDate;
                    dbObject.EventStartDate = _event.EventStartDate;
                    dbObject.EventEndDate = _event.EventEndDate;
                    dbObject.NoOfDays = _event.NoOfDays;
                    dbObject.ModifiedDate = DateTime.Now;
                    dbObject.ModifiedBy = user!.IdUser;
                    var entity = _dbContext.EventVersions.Update(dbObject);
                    await _dbContext.SaveChangesAsync();
                    var mapEntity = _mapper.Map<EventVersionDto>(entity.Entity);
                    return mapEntity;
                }
                return null;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null;
            }
        }

        public async Task<List<EventTypeDto>> GetEventTypes()
        {
            try
            {
                var eventType = await _dbContext.EventType.ToListAsync();
                return _mapper.Map<List<EventTypeDto>>(eventType);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }
    }
}
