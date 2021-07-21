using System.Collections.Generic;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

using ASP.NET.Core.WebAPI.Models.DTOs.Contracts;

namespace ASP.NET.Core.WebAPI.Models.DTOs
{
    public class AuthorDTO : BaseDTO
    {
        [Required]
        [Display(Name = "First Name")]
        [SwaggerSchema(Description = "First Name of the Author")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [SwaggerSchema(Description = "Middle Name of the Author")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [SwaggerSchema(Description = "Last Name of the Author")]
        public string LastName { get; set; }

        [SwaggerSchema(Description = "Address of the Author")]
        public string Address { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        [SwaggerSchema(Description = "Phone Number of the Author")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [SwaggerSchema(Description = "Email Address of the Author")]
        public string Email { get; set; }

        [SwaggerSchema(Description = "Books linked to the Author")]
        public List<BookDTO> Books { get; set; }
    }
}