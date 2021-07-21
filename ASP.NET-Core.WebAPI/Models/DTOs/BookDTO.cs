using System;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

using ASP.NET.Core.WebAPI.Models.DTOs.Contracts;
using ASP.NET.Core.WebAPI.Infrastructure.API.Validators;

namespace ASP.NET.Core.WebAPI.Models.DTOs
{
    public class BookDTO : BaseDTO
    {
        [Required]
        [Isbn]
        [SwaggerSchema(Description = "ISBN of the Book")]
        [Display(Name = "ISBN")]
        public string Isbn { get => isbn; set => isbn = value?.Trim().Replace("-", string.Empty) ?? string.Empty; }

        private string isbn;    // Backing field, during 'set', once Attribute Validation
                                // succeeds, need to make it 13 character long, otherwise
                                // error will occur during inserting in Db.

        [Required]
        [SwaggerSchema(Description = "Title of the Book")]
        public string Title { get; set; }

        [SwaggerSchema(Description = "Summary of the Book")]
        public string Summary { get; set; }

        [Required]
        [Display(Name = "Published On")]
        [SwaggerSchema(Description = "Date of Publish, format: yyyy-MM-dd; example: 2021-06-01", Format = "date")]
        // TODO: Make it DateOnly when migrating to .NET 6
        public DateTime PublishedOn { get; set; }

        [Required]
        [SwaggerSchema(Description = "Id of the Author", ReadOnly = true)]
        public int AuthorId { get; set; }
    }
}