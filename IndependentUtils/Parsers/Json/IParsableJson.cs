using System.Text.Json;

namespace MichaelKjellander.IndependentUtils.Parsers.Json;

public interface IParsableJson
{
    public IParsableJson ParseFromJson(JsonElement el);
    
}