using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ASP.NET.Core.WebAPI.Models.UtilityModels;

/// <summary>
/// Request type for APIs supporting sorting.
/// </summary>
public class SortingRequest
{
    [Display(Name = "Sort by Attribute")]
    [SwaggerParameter("Attribute name, based on this sorting will be done, default is 'Id'")]
    public string SortByAttribute { get; set; } = "Id";

    [Display(Name = "Sort order")]
    [SwaggerParameter("Order of sorting [ASC / DESC], default is 'ASC'")]
    public SortOrder SortByOrder { get; set; } = SortOrder.ASC;
}

public enum SortOrder { ASC, DESC }