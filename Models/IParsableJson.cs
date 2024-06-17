using System.Text.Json;

namespace MichaelKjellander.Models;

public interface IParsableJson
{
    public IParsableJson ParseFromJson(JsonElement el);
    
}