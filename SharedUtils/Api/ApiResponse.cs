namespace MichaelKjellander.SharedUtils.Api;

public class ApiResponse<T> where T : IParsableJson
{
    public ICollection<T> Items  {get ; private set; }
    public PaginationData PaginationData {get ; private set;  }

    public ApiResponse(ICollection<T> items, PaginationData paginationData)
    {
        this.Items = items;
        this.PaginationData = paginationData;
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