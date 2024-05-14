using InfoMed.DTO;

namespace InfoMed.Services.Interface
{
    public interface IEventService
    {
        public Task<List<EventVersionDto>> GetEvents();
        public Task<bool> AddEvent(EventVersionDto _event,string userId);
        public Task<bool> UpdateEvent(EventVersionDto _event,string userId);
    }
}
