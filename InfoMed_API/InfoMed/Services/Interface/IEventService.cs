using InfoMed.DTO;
using InfoMed.Models;

namespace InfoMed.Services.Interface
{
    public interface IEventService
    {
        public Task<List<EventVersionDto>> GetEvents();
        public Task<EventVersionDto> AddEvent(EventVersionDto _event,string userId);
        public Task<EventVersionDto> UpdateEvent(EventVersionDto _event,string userId);
        public Task<EventVersionDto> GetEventById(int id);
        public Task<List<EventTypeDto>> GetEventTypes();
    }
}
