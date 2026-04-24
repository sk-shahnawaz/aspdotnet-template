namespace ASP.NET.Core.WebAPI.Models.UtilityModels;

/// <summary>
/// This type is used as an API response for the endpoints which provides pagination support.
/// </summary>
/// <typeparam name="T">Generic type argument, representing a List DTO instances.</typeparam>
public class PaginationResponse<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get { return PageSize > 0 ? Convert.ToInt32(Math.Ceiling(TotalItems / (double)PageSize)) : 0; } }
    public T Data { get; set; }

    public PaginationResponse(int pageNumber, int pageSize, int totalItemsCount, T data)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalItems = totalItemsCount;
        Data = data;
    }
}