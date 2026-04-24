using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("NET-Core.XUnit.UnitTests")]
namespace ASP.NET.Core.WebAPI.Models.DTOs.Contracts;

public class BaseDTO
{
    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever]
    [Swashbuckle.AspNetCore.Annotations.SwaggerSchema(Description = "Id", ReadOnly = true)]
    [Newtonsoft.Json.JsonProperty(Order = -2)]

        /*
			Order = -2 : This will ensure 'Id' property of BaseDTO will be serialized
			before serilialization of any derived class property. The 'Id' property
			being common and important, will show up first in returned JSON structure.
			See https://stackoverflow.com/a/45690162 
		*/
    public long Id { get; internal set; }

        /*
			To pass resource ID to clients which they will use for further API calls.
			Must never be allowed to take participate in model binding to prevent 
			Over-posting attack.
		 */

    [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever]
    [Swashbuckle.AspNetCore.Annotations.SwaggerSchema(Description = "HATEOAS Links", ReadOnly = true)]
    [Newtonsoft.Json.JsonProperty(PropertyName = "_links")]
    public List<LinkDTO> Links { get; set; }

    /* 
        For HATEOAS complaint API response
     */
}