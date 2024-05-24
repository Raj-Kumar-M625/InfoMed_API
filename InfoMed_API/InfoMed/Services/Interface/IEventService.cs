using InfoMed.DTO;
using InfoMed.Models;

namespace InfoMed.Services.Interface
{
    public interface IEventService
    {
        public Task<List<EventVersionDto>> GetEvents();
        public Task<EventVersionDto> AddEvent(EventVersionDto _event,string userId);
        public Task<EventVersionDto> UpdateEvent(EventVersionDto _event,string userId);
        public Task<List<SponsersDto>> GetSponser(int eventId);       
        public Task<bool> UpdateSponser(SponsersDto _sponser,string userId);        
        public Task<EventVersionDto> GetEventById(int id);
        public Task<List<EventTypeDto>> GetEventTypes();
        public Task<List<SponserTypeDto>> GetSponserTypes();        
        public Task<SponsersDto> GetSponserById(int id);        
        public Task<bool> AddSponser(SponsersDto _sponser, string userId);
    }
}
