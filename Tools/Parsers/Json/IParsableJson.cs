using System.Text.Json;

namespace MichaelKjellander.Tools.Parsers.Json;

public interface IParsableJson
{
    public IParsableJson ParseFromJson(JsonElement el);
    
}