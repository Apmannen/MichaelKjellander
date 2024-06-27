namespace MichaelKjellander.Models;

public static class ModelFactory
{
    public static ApiResponse<T> CreateSimpleApiResponse<T>(IList<T> items) where T : DbModel
    {
        return CreateApiResponse(items, currentPage: 1, totalCount: items.Count, perPage: -1);
    }

    public static ApiResponse<T> CreateApiResponse<T>(IList<T> items, int currentPage, int totalCount, int perPage) where T : DbModel
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