using AutoMapper;
using InfoMed.DTO;
using InfoMed.Models;

namespace InfoMed.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Event, EventDto>().ReverseMap();
            CreateMap<EventType, EventTypeDto>().ReverseMap();
            CreateMap<EventVersions, EventVersionDto>().ReverseMap();
            CreateMap<TextContentAreas, TextContentAreasDto>().ReverseMap();
            CreateMap<Sponsers, SponsersDto>().ReverseMap();
            CreateMap<Speakers, SpeakersDto>().ReverseMap();
            CreateMap<SponserType, SponserTypeDto>().ReverseMap();
            CreateMap<ScheduleDetails, ScheduleDetailsDto>().ReverseMap();
            CreateMap<ScheduleMaster, ScheduleMasterDto>().ReverseMap();
            CreateMap<ConferenceFees, ConferenceFeeDto>().ReverseMap();
            CreateMap<LastYearMemory, LastYearMemoryDto>().ReverseMap();
            CreateMap<LastYearMemoryDetail, LastYearMemoryDetailDto>().ReverseMap();
            CreateMap<PaymentDetails, PaymentDetailsDto>().ReverseMap();

        }
    }
}
