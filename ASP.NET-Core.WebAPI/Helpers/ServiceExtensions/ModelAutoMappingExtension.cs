using NET.Core.Library.Domain.DBModels;
using ASP.NET.Core.WebAPI.Models.DTOs;

namespace ASP.NET.Core.WebAPI.Helpers.ServiceExtensions;

internal static class ModelAutoMappingExtension
{
    /// <summary>
    /// Registers AutoMapper mapping profiles.
    /// </summary>
    /// <param name="serviceCollection">Abstraction of type IServiceCollection</param>
    internal static void AddModelAutoMappings(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAutoMapper(configAction =>
        {
            configAction.CreateMap<Author, AuthorDTO>()
            // Example: Member of DTO type has different name than that of corresponding model type
                .ForMember(destination => destination.EmailAddress,
                           option => option.MapFrom(source => source.Email)).ReverseMap();

            configAction.CreateMap<Book, BookDTO>()
                .ForMember(destination => destination.PublishedOn,
                           option => option.MapFrom(source => DateOnly.FromDateTime(source.PublishedOn)));


            configAction.CreateMap<BookDTO, Book>()
                .ForMember(destination => destination.PublishedOn,
                           option => option.MapFrom(source => DateTime.ParseExact(source.PublishedOn.ToString(), "yyyy-MM-dd", null)));
        });
    }
}