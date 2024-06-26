﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace MichaelKjellander.Domains.Models.Wordpress;

[Table("wp_tags")]
public class WpTag : WordpressModel
{
    [MaxLength(VarcharLength)]
    public string Name { get; set; } = "";
    [MaxLength(VarcharLength)]
    public string ShortName { get; set; } = "";
    [MaxLength(VarcharLength)]
    public string Slug { get; set; } = "";
    public IList<WpPostTag>? PostTags { get; set; }
    
    public override WpTag ParseFromJson(JsonElement el)
    {
        Id = el.GetProperty("id").GetInt32();
        Name = el.GetProperty("name").GetString()!;
        Slug = el.GetProperty("slug").GetString()!;
        ShortName = el.GetProperty("description").GetString()!;

        return this;
    }
}