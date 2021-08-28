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
                    .ForMember(destination => destination.FullName,
                               option => option.MapFrom(source => !string.IsNullOrEmpty(source.MiddleName) ?
                                    $"{source.FirstName} {source.MiddleName} {source.LastName}" : $"{source.FirstName} {source.LastName}")).ReverseMap();

                configAction.CreateMap(typeof(Book), typeof(BookDTO)).ReverseMap();
            });
        }
    }
}