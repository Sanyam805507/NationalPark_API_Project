using AutoMapper;
using NationalPark_API_Project.DTOs;
using NationalPark_API_Project.Models;

namespace NationalPark_API_Project.DTOMapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<TrailDTO,Trail>().ReverseMap();
            CreateMap<NationalPark, NationalParkDTO>().ReverseMap();
        }
    }
    // SERVER----DB---MODEL---REPOSITORY----DTO----CLIENT
    // CLIENT----DTO----REPOSITORY----MODEL----DB----SERVER
}
