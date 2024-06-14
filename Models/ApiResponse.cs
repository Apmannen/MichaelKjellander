using System.Text.Json;
using MichaelKjellander.Models;
using Newtonsoft.Json;

namespace MichaelKjellander.Models;

public class ApiResponse<T> : IParsableJson
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
        var items = el.GetProperty("items").EnumerateArray();
        List<T> deserializedList = new List<T>();
        foreach (JsonElement item in items)
        {
            string jsonString = item.ToString();
            T deserialized = JsonConvert.DeserializeObject<T>(item.ToString())!;
            deserializedList.Add(deserialized);
        }
        this.Items = deserializedList;

        PaginationData = ParsePaginationData(el, Items.Count);

        return this;
    }

    private PaginationData ParsePaginationData(JsonElement el, int count)
    {
        var currentPage = el.GetProperty("paginationData").GetProperty("currentPage").GetInt32();
        var numPages = el.GetProperty("paginationData").GetProperty("numPages").GetInt32();
        return new PaginationData(currentPage, numPages, count);
    }
}

public class PaginationData
{
    public int CurrentPage {get ; private init; }
    public int NumPages {get ; private init;  }
    public int Count { get; private init; }

    public PaginationData(int currentPage, int numPages, int count)
    {
        this.CurrentPage = currentPage;
        this.NumPages = numPages;
        this.Count = count;
    }
}