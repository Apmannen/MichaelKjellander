﻿using System.Text.Json;
using MichaelKjellander.SharedUtils.Json;

namespace MichaelKjellander.Models.Wordpress;

public class WpPage : IParsableJson
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public string? Title { get; set; }
    public DateOnly Date { get; set; }
    
    public void ParseFromJson(JsonElement el)
    {
        int id = el.GetProperty("id").GetInt32();
        string content = el.GetProperty("content").GetProperty("rendered").GetString()!;
        string title = el.GetProperty("title").GetProperty("rendered").GetString()!;
        string dateString = el.GetProperty("date").GetString()!;
        DateOnly date = DateOnly.Parse(dateString);
        
        this.Id = id;
        this.Content = content;
        this.Title = title;
        this.Date = date;
    }
}