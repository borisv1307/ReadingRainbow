using ReadingRainbowAPI.Dto;
using ReadingRainbowAPI.Models;
using AutoMapper;


namespace ReadingRainbowAPI.Mapping
{

    public class MappingProfile : Profile
    {
	    public MappingProfile()
	    {
		    CreateMap<Person, PersonDto>();

	    }
    }
}