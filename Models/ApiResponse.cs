using System.Text.Json;
using MichaelKjellander.Models;
using Newtonsoft.Json;

namespace MichaelKjellander.SharedUtils.Api;

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

    public override void ParseFromJson(JsonElement el)
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
        PaginationData = new PaginationData(currentPage, numPages);
    }
}

public class PaginationData
{
    public int CurrentPage {get ; private set; }
    public int NumPages {get ; private set;  }

    public PaginationData(int currentPage, int numPages)
    {
        this.CurrentPage = currentPage;
        this.NumPages = numPages;
    }
}