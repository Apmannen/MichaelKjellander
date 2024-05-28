﻿using System.Text.Json;
using MichaelKjellander.Utils;

namespace MichaelKjellander.Models.Wordpress;

public class WpApiCategory : IParsableJson
{
    public int Id { get; private init; }
    public string Name { get; private init; }
    public static IParsableJson ParseFromJson(JsonElement el)
    {
        var id = el.GetProperty("id").GetInt32();
        var name = el.GetProperty("name").GetString();

        return new WpApiCategory{Id = id, Name = name};
    }
}