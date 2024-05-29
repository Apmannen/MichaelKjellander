using System.Text.Json;
using MichaelKjellander.SharedUtils.Json;

namespace MichaelKjellander.SharedUtils.Api;

public class ApiResponse<T> : IParsableJson where T : IParsableJson
{
    public ICollection<T>? Items  {get ; private set; }
    public PaginationData? PaginationData {get ; private set;  }

    public ApiResponse() {}
    public ApiResponse(ICollection<T> items, PaginationData paginationData)
    {
        this.Items = items;
        this.PaginationData = paginationData;
    }

    public void ParseFromJson(JsonElement el)
    {
        List<T> itemsList = JsonUtil.ParseList<T>(el.GetProperty("items"));
        this.Items = itemsList;
        
        PaginationData!.ParseFromJson(el.GetProperty("paginationData"));
    }
}

public class PaginationData : IParsableJson
{
    public int CurrentPage {get ; private set; }
    public int NumPages {get ; private set;  }

    public PaginationData(int currentPage, int numPages)
    {
        this.CurrentPage = currentPage;
        this.NumPages = numPages;
    }

    public void ParseFromJson(JsonElement el)
    {
        this.CurrentPage = el.GetProperty("currentPage").GetInt32();
        this.NumPages = el.GetProperty("numPages").GetInt32();
    }
}