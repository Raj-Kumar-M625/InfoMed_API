using AutoMapper;
using InfoMed.Data;
using InfoMed.DTO;
using InfoMed.Models;
using InfoMed.Services.Interface;
using InfoMed.Utils;
using log4net;
using Microsoft.Data.SqlClient;
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

        public async Task<List<EventVersionDto>> GetEvents(string version)
        {
            try
            {
                var events = await _dbContext.EventVersions.ToListAsync();
              var mappedEvents = _mapper.Map<List<EventVersionDto>>(events);
                if (events!= null)
                {
                    foreach(var item in mappedEvents) {
                        var createdByString = await _dbContext.EventsMaster
                                          .Where(x => x.IdEvent == item.IdEvent).FirstOrDefaultAsync();                                        
                        item.CreatedBy = createdByString?.CreatedBy;
                    }

                }

                if(version == Constants.LatestVersion)
                {
                    return mappedEvents.GroupBy(x => x.EventName).Select(x => x.LastOrDefault()).ToList();
                }

                return mappedEvents;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }

        public async Task<List<SponsersDto>> GetSponser(int id, int idVersion)
        {
            try
            {
                var events = await _dbContext.Sponsors .Where(x=>x.IdEvent== id && x.IdEventVersion == idVersion && x.Status==true).OrderBy(x=>x.OrderNumber).ToListAsync();
                return _mapper.Map<List<SponsersDto>>(events);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }

        public async Task<List<SpeakersDto>> GetSpeakers(int id, int idVersion)
        {
            try
            {
                var events = await _dbContext.Speakers.Where(x => x.IdEvent == id && x.IdEventVersion ==idVersion  && x.Status == true).OrderBy(x => x.OrderNumber).ToListAsync();
                return _mapper.Map<List<SpeakersDto>>(events);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }
        



        public async Task<EventVersionDto> GetEventById(int id, int idVersion)
        {
            try
            {
                var _event = await _dbContext.EventVersions.FirstOrDefaultAsync(x => x.IdEventVersion == idVersion && x.IdEvent == id);
                return _mapper.Map<EventVersionDto>(_event);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }

        public async Task<SponsersDto> GetSponserById(int id)
        {
            try
            {
                var _event = await _dbContext.Sponsors.FirstOrDefaultAsync(x => x.IdEventSponsor == id);
                return _mapper.Map<SponsersDto>(_event);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }

        public async Task<int> EventVersionCreate(int id, string email)
        {
            try
            {
                int newEventVersionId;
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.EmailAddress == email);
                var userid = user!.IdUser;
                var commandText = "EXEC [dbo].[CreateNewDraftVersionOfEvent] @IdEvent, @IdUser";

                using (var connection = _dbContext.Database.GetDbConnection())
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = commandText;
                        command.Parameters.Add(new SqlParameter("@IdEvent", id));
                        command.Parameters.Add(new SqlParameter("@IdUser", userid));

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                newEventVersionId = reader.GetInt32(0);
                            }
                            else
                            {
                                newEventVersionId = 0; // handle error or default value
                            }
                        }
                    }
                }
                //var _event = await _dbContext.EventVersions.FirstOrDefaultAsync(x => x.IdEventVersion == newEventVersionId);
                //    if (_event != null)
                //{
                //    newEventVersionId = _event.IdVersion;
                //}
                return newEventVersionId;

            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return 0!;
            }
        }

        

        public async Task<SpeakersDto> GetSpeakerById(int id)
        {
            try
            {
                var _event = await _dbContext.Speakers.FirstOrDefaultAsync(x => x.IdSpeaker == id);
                return _mapper.Map<SpeakersDto>(_event);
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

                    //var prevEvent = await _dbContext.EventVersions.OrderBy(x => x.IdEventVersion).LastOrDefaultAsync();

                    var newEvent = _mapper.Map<EventVersions>(_event);
                    newEvent.IdEvent = result.Entity.IdEvent;
                    newEvent.IdVersion =  1;

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

        public async Task<bool> AddSponser(SponsersDto _sponser, string userId)
        {
            try
            {
                var sponsers = _mapper.Map<Sponsers>(_sponser);
                var existingsponsers= await _dbContext.Sponsors
                   .Where(tc => tc.OrderNumber >= sponsers.OrderNumber && tc.IdEvent == sponsers.IdEvent && tc.IdEventVersion == sponsers.IdEventVersion)
                   .OrderBy(tc => tc.OrderNumber)
                   .ToListAsync();
                if (existingsponsers.Any())
                {
                    foreach (var content in existingsponsers)
                    {
                        content.OrderNumber++;
                    }

                    _dbContext.Sponsors.UpdateRange(existingsponsers);
                }

                await _dbContext.Sponsors.AddAsync(sponsers);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return false;
            }
        }

        public async Task<bool> AddSpeaker(SpeakersDto _speaker, string userId)
        {
            try
            {
                var speakers = _mapper.Map<Speakers>(_speaker);
                var existingspeakers = await _dbContext.Speakers
                 .Where(tc => tc.OrderNumber >= speakers.OrderNumber && tc.IdEvent == speakers.IdEvent && tc.IdEventVersion == speakers.IdEventVersion && speakers.Status== true)
                 .OrderBy(tc => tc.OrderNumber)
                 .ToListAsync();
                if (existingspeakers.Any())
                {
                    foreach (var content in existingspeakers)
                    {
                        content.OrderNumber++;
                    }

                    _dbContext.Speakers.UpdateRange(existingspeakers);
                }
                await _dbContext.Speakers.AddAsync(speakers);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return false;
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
                    dbObject.VersionStatus = _event.VersionStatus;
                    dbObject.EventWebPageName = _event.EventWebPageName;
                    dbObject.EventName = _event.EventName;
                    dbObject.EventBackgroundImage = _event.EventBackgroundImage;
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
                    dbObject.ShowPayAtVenueButton = _event.ShowPayAtVenueButton;
                    dbObject.EventBackgroundColor = _event.EventBackgroundColor;
                    dbObject.ShowPayNowButton = _event.ShowPayNowButton;
                    dbObject.ApprovedBy = _event.ApprovedBy;
                    dbObject.ApprovedDate = _event.ApprovedDate;
                    dbObject.EventStartDate = _event.EventStartDate;
                    dbObject.EventEndDate = _event.EventEndDate;
                    dbObject.NoOfDays = _event.NoOfDays;
                    dbObject.ModifiedDate = DateTime.Now;
                    dbObject.ModifiedBy = user!.IdUser;
                    if(_event.VersionStatus== "Approved")
                    {
                        dbObject.ApprovedBy = user!.IdUser;
                        dbObject.ApprovedDate =DateTime.Now;
                    }
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

        public async Task<List<SponserTypeDto>> GetSponserTypes()
        {
            try
            {
                var sponserType = await _dbContext.SponserType.ToListAsync();
                return _mapper.Map<List<SponserTypeDto>>(sponserType);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }

        public async Task<bool> UpdateSponser(SponsersDto _Sponser, string userId)
        {
            try
            {                
                if (_Sponser.Status != false)
                {
                    var spoobject = await _dbContext.Sponsors.FirstOrDefaultAsync(x => x.IdEventSponsor == _Sponser.IdEventSponsor);
                    if(spoobject.OrderNumber != _Sponser.OrderNumber)
                    {
                        var existingSponser = await _dbContext.Sponsors
                                       .Where(tc => tc.OrderNumber == _Sponser.OrderNumber && tc.IdEvent == _Sponser.IdEvent && tc.IdEventVersion == _Sponser.IdEventVersion && tc.Status == true)
                                       .OrderBy(tc => tc.OrderNumber)
                                       .ToListAsync();

                        if (existingSponser.Any())
                        {
                            foreach (var content in existingSponser)
                            {
                                content.OrderNumber++;
                            }

                            _dbContext.Sponsors.UpdateRange(existingSponser);
                        }
                    }
                    
                }

                var dbObject = await _dbContext.Sponsors.FirstOrDefaultAsync(x => x.IdEventSponsor == _Sponser.IdEventSponsor);
                if (dbObject != null)
                {
                    dbObject.IdEvent = _Sponser.IdEvent;
                    dbObject.IdEventVersion = _Sponser.IdEventVersion;
                    dbObject.SponsorType = _Sponser.SponsorType;
                    dbObject.SponsorName = _Sponser.SponsorName;
                    dbObject.SponsorUrl = _Sponser.SponsorUrl;
                    dbObject.SponsorShowText = _Sponser.SponsorShowText;
                    dbObject.OrderNumber = _Sponser.OrderNumber;
                    dbObject.SponsorLogo = _Sponser.SponsorLogo;
                    dbObject.Status = _Sponser.Status;
                    _dbContext.Sponsors.Update(dbObject);
                    await _dbContext.SaveChangesAsync();
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

        public async Task<bool> UpdateSpeaker(SpeakersDto _speaker, string userId)
        {
            try
            {
                if (_speaker.Status != false)
                {
                    var usObject = await _dbContext.Speakers.FirstOrDefaultAsync(x => x.IdSpeaker == _speaker.IdSpeaker);
                    if (usObject.OrderNumber != _speaker.OrderNumber)
                    {
                        var existingSpeaker = await _dbContext.Speakers
                                       .Where(tc => tc.OrderNumber >= _speaker.OrderNumber && tc.IdEvent == _speaker.IdEvent && tc.IdEventVersion == _speaker.IdEventVersion && tc.Status == true)
                                       .OrderBy(tc => tc.OrderNumber)
                                       .ToListAsync();
                        if (existingSpeaker.Any())
                        {
                            foreach (var content in existingSpeaker)
                            {
                                content.OrderNumber++;
                            }

                            _dbContext.Speakers.UpdateRange(existingSpeaker);
                        }
                    }                      
                }

                var dbObject = await _dbContext.Speakers.FirstOrDefaultAsync(x => x.IdSpeaker == _speaker.IdSpeaker);
                if (dbObject != null)
                {
                    dbObject.IdEvent = _speaker.IdEvent;
                    dbObject.IdEventVersion = _speaker.IdEventVersion;
                    dbObject.SpeakerName = _speaker.SpeakerName;
                    dbObject.AboutSpeaker = _speaker.AboutSpeaker;
                    dbObject.OrderNumber = _speaker.OrderNumber;
                    dbObject.SpeakerImage = _speaker.SpeakerImage;
                    dbObject.Status = _speaker.Status;
                    _dbContext.Speakers.Update(dbObject);
                    await _dbContext.SaveChangesAsync();                  
                    
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

        public async Task<bool> DeleteSponsor(int Id)
        {
            try
            {
                var dbObject = await _dbContext.Sponsors.FirstOrDefaultAsync(x => x.IdEventSponsor == Id);
                if (dbObject != null)
                {
                    dbObject.Status = false;
                    _dbContext.Sponsors.Update(dbObject);
                    await _dbContext.SaveChangesAsync();
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

        public async Task<bool> DeleteSpeaker(int Id)
        {
            try
            {
                var dbObject = await _dbContext.Speakers.FirstOrDefaultAsync(x => x.IdSpeaker == Id);
                if (dbObject != null)
                {
                    dbObject.Status = false;
                    _dbContext.Speakers.Update(dbObject);
                    await _dbContext.SaveChangesAsync();
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

        public async Task<EventVersionDto> GetEventByName(string webPageName)
        {
            try
            {
                var _event = await _dbContext.EventVersions.FirstOrDefaultAsync(x => EF.Functions.Like(x.EventWebPageName, webPageName));
                return _mapper.Map<EventVersionDto>(_event);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null!;
            }
        }

        public async Task<EventViewModel> GetEventDetails(string webPageName)
        {
            try
            {
                //var _events = await _dbContext.EventVersions.Where(x => EF.Functions.Like(x.EventWebPageName, webPageName) && x.VersionStatus.ToLower().Trim() == "approved").ToListAsync();
                //var _event = _events.Count() > 0 ? _events.OrderByDescending(x => x.IdVersion).First():null;
                var _event = new EventVersions();
                if (webPageName == null)
                {
                    _event = await _dbContext.EventVersions.Where(x => x.VersionStatus.ToLower().Trim() == "approved")
                                                                  .OrderByDescending(x => x.IdEventVersion).FirstAsync();
                }
                else{ 
                    _event = await _dbContext.EventVersions
                    .Where(x => EF.Functions.Like(x.EventWebPageName, webPageName) && x.VersionStatus.ToLower().Trim() == "approved")
                    .OrderByDescending(x => x.IdVersion)
                    .FirstOrDefaultAsync();
                }

                if (_event != null)
                {
                    var textContextAreas = await _dbContext.TextContentAreas.Where(x => x.IdEvent == _event.IdEvent && x.IdEventVersion == _event.IdEventVersion && x.Status == true).ToListAsync();
                    var sponcers = await _dbContext.Sponsors.Where(x => x.IdEvent == _event.IdEvent && x.IdEventVersion == _event.IdEventVersion && x.Status == true).OrderBy(x => x.OrderNumber).ToListAsync();
                    var speakers = await _dbContext.Speakers.Where(x => x.IdEvent == _event.IdEvent && x.IdEventVersion == _event.IdEventVersion && x.Status == true).ToListAsync();
                    var schedule = await _dbContext.ScheduleMaster.Where(x => x.IdEvent == _event.IdEvent && x.IdEventVersion == _event.IdEventVersion && x.IsActive == true).ToListAsync();
                    var conferenceFees = await _dbContext.ConferenceFees.Where(x => x.IdEvent == _event.IdEvent && x.IdEventVersion == _event.IdEventVersion && x.IsActive == true).ToListAsync();
                    var lastYearMemories = await _dbContext.LastYearMemories.Where(x => x.IdEvent == _event.IdEvent && x.IdEventVersion == _event.IdEventVersion && x.Status == true ).ToListAsync();
                    var paymentDetails = await _dbContext.PaymentDetails.FirstOrDefaultAsync(x => x.IdEvent == _event.IdEvent && x.IdEventVersion == _event.IdEventVersion);

                    EventViewModel eventViewModel = new EventViewModel();
                    eventViewModel.EventVersion = _mapper.Map<EventVersionDto>(_event);
                    eventViewModel.TextContentAreas = _mapper.Map<List<TextContentAreasDto>>(textContextAreas);
                    eventViewModel.Sponsers = _mapper.Map<List<SponsersDto>>(sponcers);
                    eventViewModel.Speakers = _mapper.Map<List<SpeakersDto>>(speakers);
                    eventViewModel.ScheduleMaster = _mapper.Map<List<ScheduleMasterDto>>(schedule);
                    eventViewModel.ConferenceFee = _mapper.Map<List<ConferenceFeeDto>>(conferenceFees);
                    eventViewModel.LastYearMemory = _mapper.Map<List<LastYearMemoryDto>>(lastYearMemories);
                    eventViewModel.PaymentDetails = _mapper.Map<PaymentDetailsDto>(paymentDetails);

                    foreach (var obj in eventViewModel.ScheduleMaster)
                    {
                        var scheduleDetailsDtos = await _dbContext.ScheduleDetails
                                                                  .Where(x => x.IdScheduleMaster == obj.IdScheduleMaster && x.IsActive == true)
                                                                  .OrderBy(x => x.StartTime.TimeOfDay)
                                                                  .ToListAsync();
                        obj.ScheduleDetailsDtos = _mapper.Map<List<ScheduleDetailsDto>>(scheduleDetailsDtos);
                    }

                    foreach(var obj in eventViewModel.LastYearMemory)
                    {
                        var lastYearMemoryDetails = await _dbContext.LastYearMemoryDetails
                                                                  .Where(x => x.IdLastYearMemory == obj.IdLastYearMemory) 
                                                                  .ToListAsync();
                        obj.LastYearMemoryDetails = _mapper.Map<List<LastYearMemoryDetailDto>>(lastYearMemoryDetails);
                    }

                    return eventViewModel;
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
