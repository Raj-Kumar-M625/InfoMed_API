using InfoMed.DTO;

namespace InfoMed.Services.Interface
{
    public interface IEventService
    {
        public Task<List<EventDto>> GetEvents();
    }
}
