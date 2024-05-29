using MichaelKjellander.SharedUtils.Api;

namespace MichaelKjellander.Data;

public class InternalApiContext
{
    private readonly HttpClient _client;

    public InternalApiContext(HttpClient client)
    {
        this._client = client;
    }

    public void Fetch(string path)
    {
        
    }
}