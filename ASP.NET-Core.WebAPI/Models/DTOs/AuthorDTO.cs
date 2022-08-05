﻿using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

using ASP.NET.Core.WebAPI.Models.DTOs.Contracts;

namespace ASP.NET.Core.WebAPI.Models.DTOs;

public class AuthorDTO : BaseDTO
{
    [Required]
    [Display(Name = "First Name")]
    [SwaggerSchema(Description = "First Name of the Author")]
    public string FirstName { get; init; }

    [Display(Name = "Middle Name")]
    [SwaggerSchema(Description = "Middle Name of the Author")]
    public string MiddleName { get; init; }

    [Required]
    [Display(Name = "Last Name")]
    [SwaggerSchema(Description = "Last Name of the Author")]
    public string LastName { get; init; }

    [Display(Name = "Full Name")]
    [SwaggerSchema(Description = "Full Name of the Author", ReadOnly = true, Nullable = false)]
    public string FullName { get => !string.IsNullOrEmpty(MiddleName) ? $"{FirstName} {MiddleName} {LastName}" : $"{FirstName} {LastName}"; }

    [SwaggerSchema(Description = "Address of the Author")]
    public string Address { get; init; }

    [Required]
    [Phone]
    [Display(Name = "Phone Number")]
    [SwaggerSchema(Description = "Phone Number of the Author")]
    public string PhoneNumber { get; init; }

    [Required]
    [EmailAddress]
    [SwaggerSchema(Description = "Email Address of the Author")]
    public string EmailAddress { get; init; }

    [SwaggerSchema(Description = "Books linked to the Author")]
    public List<BookDTO> Books { get; internal set; }
}