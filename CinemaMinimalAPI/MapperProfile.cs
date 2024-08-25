using AutoMapper;
using CinemaMinimalAPI.DTO;
using CinemaMinimalAPI.Models;

namespace CinemaMinimalAPI;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Movie, MovieDTO>().ReverseMap();
        CreateMap<AddMovieDTO, Movie>().ReverseMap();
        CreateMap<CinemaDTO, Cinema>().ReverseMap();
        CreateMap<Session, SessionDTO>().ReverseMap();
        CreateMap<AddSessionDTO, Session>();
    }
}