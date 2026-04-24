using AutoMapper;
using NET.Core.Library.Domain.DBModels;
using ASP.NET.Core.WebAPI.Models.DTOs;

namespace ASP.NET.Core.WebAPI.Helpers.ServiceExtensions;

internal sealed class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Author, AuthorDTO>()
            .ForMember(destination => destination.EmailAddress,
                       option => option.MapFrom(source => source.Email)).ReverseMap();

        CreateMap<Book, BookDTO>()
            .ForMember(destination => destination.PublishedOn,
                       option => option.MapFrom(source => DateOnly.FromDateTime(source.PublishedOn)));

        CreateMap<BookDTO, Book>()
            .ForMember(destination => destination.PublishedOn,
                       option => option.MapFrom(source => DateTime.ParseExact(source.PublishedOn.ToString(), "yyyy-MM-dd", null)));
    }
}
