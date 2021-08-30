using Microsoft.Extensions.DependencyInjection;

using ASP.NET.Core.WebAPI.Models.DTOs;
using NET.Core.Library.Domain.DBModels;

namespace ASP.NET.Core.WebAPI.Helpers.ServiceExtensions
{
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

                configAction.CreateMap(typeof(Book), typeof(BookDTO)).ReverseMap();
            });
        }
    }
}