using InfoMed.DTO;

namespace InfoMed.Services.Interface
{
    public interface ILastYearMemoriesService
    {
        public Task<List<LastYearMemoryDto>> GetLastYearMemoriesList(int eventId);
        public Task<LastYearMemoryDto> AddLastYearMemories(LastYearMemoryDto LastYearMemoryDto);
      public Task<LastYearMemoryDto> UpdateLastYearMemories(LastYearMemoryDto LastYearMemoryDto);
        public Task<LastYearMemoryDto> GetLastYearMemoriesById(int idLastYearMemory);
        

    }
}
