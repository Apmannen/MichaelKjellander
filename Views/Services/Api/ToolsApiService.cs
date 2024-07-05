using MichaelKjellander.Config;
using MichaelKjellander.Domains.ApiResponse;
using Microsoft.Extensions.Options;

namespace MichaelKjellander.Views.Services.Api;

public class ToolsApiService : InternalApiService
{
    public ToolsApiService(HttpClient client, IOptions<AppConfig> options) : base(client, options)
    {
    }

    public async Task<IList<int>> PostSolveSudoku(int[] values)
    {
        object payload = new { values = values };
        ApiResponse<int>
            response = await PostFetchGeneric<int>(ApiRoutes.SudokuSolve, payload);
        return response.Items;
    }
}