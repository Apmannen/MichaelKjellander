using System.Text.Json;
using MichaelKjellander.Models;
using Newtonsoft.Json;

namespace MichaelKjellander.Models;

public class ApiResponse<T> : Model where T : Model
{
    public IList<T>? Items  {get ; private set; }
    public PaginationData? PaginationData {get ; private set;  }

    public ApiResponse() {}
    public ApiResponse(IList<T> items, PaginationData paginationData)
    {
        this.Items = items;
        this.PaginationData = paginationData;
    }

    public override Model ParseFromJson(JsonElement el)
    {
        var items = el.GetProperty("items").EnumerateArray();
        List<T> deserializedList = new List<T>();
        foreach (var item in items)
        {
            T deserialized = JsonConvert.DeserializeObject<T>(item.ToString())!;
            deserializedList.Add(deserialized);
        }

        this.Items = deserializedList;

        var currentPage = el.GetProperty("paginationData").GetProperty("currentPage").GetInt32();
        var numPages = el.GetProperty("paginationData").GetProperty("numPages").GetInt32();
        PaginationData = new PaginationData(currentPage, numPages, Items.Count);

        return this;
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