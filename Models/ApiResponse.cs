using System.Text.Json;
using MichaelKjellander.Models;
using MichaelKjellander.Tools.Parsers;
using MichaelKjellander.Tools.Parsers.Json;
using Newtonsoft.Json;

namespace MichaelKjellander.Models;

public class ApiResponse<T> : IParsableJson where T : DbModel
{
    public IList<T>? Items  {get ; private set; }
    public PaginationData? PaginationData {get ; private set;  }

    public ApiResponse() {}
    public ApiResponse(IList<T> items, PaginationData paginationData)
    {
        this.Items = items;
        this.PaginationData = paginationData;
    }

    public void ParseStringsFromJson(JsonElement el)
    {
        var items = el.GetProperty("items").EnumerateArray();
        List<string> list = [];
        foreach (JsonElement item in items)
        {
            list.Add(item.ToString());
        }
        this.Items = list as List<T>;
        PaginationData = ParsePaginationData(el, Items!.Count);
    }

    public IParsableJson ParseFromJson(JsonElement el)
    {
        this.Items = JsonParser.DeserializeObjectCollection<T>(el.GetProperty("items").EnumerateArray());
        PaginationData = ParsePaginationData(el, Items.Count);

        return this;
    }

    private PaginationData ParsePaginationData(JsonElement el, int count)
    {
        int currentPage = el.GetProperty("paginationData").GetProperty("currentPage").GetInt32();
        int numPages = el.GetProperty("paginationData").GetProperty("numPages").GetInt32();
        int totalCount = el.GetProperty("paginationData").GetProperty("totalCount").GetInt32();
        return new PaginationData(currentPage, numPages, count, totalCount);
    }
}

public class PaginationData
{
    public int CurrentPage {get ; private init; }
    public int NumPages {get ; private init;  }
    public int Count { get; private init; }
    public int TotalCount { get; private init;  }

    public PaginationData(int currentPage, int numPages, int count, int totalCount)
    {
        this.CurrentPage = currentPage;
        this.NumPages = numPages;
        this.Count = count;
        this.TotalCount = totalCount;
    }
}