using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ASP.NET.Core.WebAPI.Models.UtilityModels
{
    public class PaginationRequest
    {
        [Display(Name = "Page Number")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid page number, Minimum:1")]
        [SwaggerParameter("Pagination page number, default is '1'")]
        public int PageNumber { get; set; } = 1;

        [Display(Name = "Page Size")]
        [SwaggerParameter("Pagination page size, default is '10'")]
        [Range(1, 100, ErrorMessage = "Invalid page size, Minimum:1 and Maximum:100")]
        public int PageSize { get; set; } = 10;
    }
}