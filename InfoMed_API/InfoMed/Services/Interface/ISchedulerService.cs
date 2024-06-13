using InfoMed.DTO;

namespace InfoMed.Services.Interface
{
    public interface ISchedulerService
    {
        public Task<List<ScheduleMasterDto>> GetSchedulesMaster(int id, int idVersion);
        public Task<List<ScheduleDetailsDto>> GetSchedulesDetails(int IdScheduleMaster);
        public Task<ScheduleMasterDto> AddScheduleMaster(ScheduleMasterDto ScheduleMasterDto);
        public Task<ScheduleDetailsDto> AddScheduleDetails(ScheduleDetailsDto ScheduleDetailsDto);
        public Task<ScheduleMasterDto> UpdateScheduleMaster(ScheduleMasterDto scheduleMasterDto);
        public Task<ScheduleDetailsDto> UpdateScheduleDetails(ScheduleDetailsDto scheduleDetailsDto);
        public Task<ScheduleDetailsDto> GetScheduleDetailById(int idScheduleDetail);
        public Task<ScheduleMasterDto> GetScheduleMasterById(int idScheduleMaster);
    }
}
