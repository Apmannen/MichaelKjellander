using System.Text.Json;

namespace MichaelKjellander.SharedUtils.Json;

public interface IParsableJson
{
    public void ParseFromJson(JsonElement el);
}