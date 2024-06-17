using System.Net.Http.Headers;
using System.Text.Json;

namespace MichaelKjellander.IndependentUtils.Api;

public class JsonFetchResult(JsonElement root, HttpResponseHeaders headers)
{
    public readonly JsonElement Root = root;
    public readonly HttpResponseHeaders Headers = headers;
}