using InfoMed.DTO;
using InfoMed.Models;

namespace InfoMed.Services.Interface
{
    public interface IEventService
    {
        public Task<List<EventVersionDto>> GetEvents();
        public Task<EventVersionDto> AddEvent(EventVersionDto _event,string userId);
        public Task<EventVersionDto> UpdateEvent(EventVersionDto _event,string userId);
        public Task<List<SponsersDto>> GetSponser(int id, int idVersion);       
        public Task<List<SpeakersDto>> GetSpeakers(int id, int idVersion);
        public Task<bool> UpdateSponser(SponsersDto _sponser,string userId);        
        public Task<bool> UpdateSpeaker(SpeakersDto _sponser,string userId);        
        public Task<EventVersionDto> GetEventById(int id,int idVersion);
        public Task<List<EventTypeDto>> GetEventTypes();
        public Task<List<SponserTypeDto>> GetSponserTypes();        
        public Task<SponsersDto> GetSponserById(int id);  
        public Task<SpeakersDto> GetSpeakerById(int id);  
        public Task<bool> DeleteSponsor(int id);
        public Task<bool> DeleteSpeaker(int id);
        public Task<bool> AddSponser(SponsersDto _sponser, string userId);
        public Task<bool> AddSpeaker(SpeakersDto _sponser, string userId);
        
    }
}
