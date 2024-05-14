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
            CreateMap<EventVersions, EventVersionDto>().ReverseMap();
        }
    }
}
