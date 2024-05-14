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

        public async Task<bool> AddEvent(EventVersionDto _event,string userId)
        {
            try
            {
                var newEvent = _mapper.Map<EventVersions>(_event);
                newEvent.ModifiedDate = DateTime.Now;
                newEvent.ModifiedBy = int.Parse(userId);
                await _dbContext.EventVersions.AddAsync(newEvent);
                return true;
            }catch(Exception ex)
            {
                _log.Error(ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateEvent(EventVersionDto _event, string userId)
        {
            try
            {
                var dbObject = await _dbContext.EventVersions.FirstOrDefaultAsync(x => x.IdEventVersion == _event.IdEventVersion);
                if (dbObject != null)
                {
                    dbObject.IdEvent = _event.IdEvent;
                    dbObject.IdVersion = _event.IdVersion;
                    dbObject.VersionStatus = _event.VersionStatus;
                    dbObject.EventWebPageName = _event.EventWebPageName;
                    dbObject.EventName = _event.EventName;
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
                    dbObject.ModifiedBy = int.Parse(userId);
                    _dbContext.EventVersions.Update(dbObject);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return false;
            }
        }
    }
}
