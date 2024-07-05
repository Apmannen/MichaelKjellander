using MichaelKjellander.Domains.Models;

namespace MichaelKjellander.Domains.ApiResponse;

//TODO: should be more like a builder class, and change its name
public static class ApiResponseFactory
{
    public static ApiResponse<T> CreateSimpleApiResponse<T>(IList<T> items)
    {
        return CreateApiResponse(items, currentPage: 1, totalCount: items.Count, perPage: -1);
    }

    public static ApiResponse<T> CreateApiResponse<T>(IList<T> items, int currentPage, int totalCount, int perPage)
    {
        int numPages = 1;
        if (perPage >= 0)
        {
            numPages = (int)Math.Ceiling((float)totalCount / (float)perPage);
        }

        return new ApiResponse<T>(items,
            new PaginationData(currentPage, numPages, items.Count, totalCount));
    }
}