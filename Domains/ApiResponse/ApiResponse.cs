using System.Text.Json;
using MichaelKjellander.Domains.Models;
using MichaelKjellander.IndependentUtils.Parsers.Json;

namespace MichaelKjellander.Domains.ApiResponse;

public class ApiResponse<T> : IParsableJson where T : DbModel
{
    public IList<T> Items { get; private set; }
    public PaginationData PaginationData { get; private set; }

    /// <summary>
    /// Needed for parameterless construction
    /// </summary>
    public ApiResponse()
    {
    }

    public ApiResponse(IList<T> items, PaginationData paginationData)
    {
        this.Items = items;
        this.PaginationData = paginationData;
    }

    public IParsableJson ParseFromJson(JsonElement el)
    {
        this.Items = JsonParser.DeserializeObjectCollection<T>(el.GetProperty("items").EnumerateArray());
        this.PaginationData = JsonParser.DeserializeObject<PaginationData>(el.GetProperty("paginationData"));

        return this;
    }
}

public class PaginationData
{
    public int CurrentPage { get; private init; }
    public int NumPages { get; private init; }
    public int Count { get; private init; }
    public int TotalCount { get; private init; }
    public Dictionary<string, Dictionary<string, int>> FieldCounts { get; private init; }

    public PaginationData(int currentPage, int numPages, int count, int totalCount)
    {
        this.CurrentPage = currentPage;
        this.NumPages = numPages;
        this.Count = count;
        this.TotalCount = totalCount;
        this.FieldCounts = new Dictionary<string, Dictionary<string, int>>();
    }
    
    public void AddCounter(string name, List<KeyValuePair<string, int>> list)
    {
        FieldCounts[name] = new Dictionary<string, int>();
        foreach (var item in list)
        {
            string key = item.Key;
            if (string.IsNullOrEmpty(key))
            {
                key = "_";
            }

            int count = item.Value;
            FieldCounts[name].Add(key, count);
        }
    }
}