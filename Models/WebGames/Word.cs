﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace MichaelKjellander.Models.WebGames;

public class Word : Model
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; }
    [StringLength(50)]
    [Required]
    public string? WordString { get; set; }

    public override void ParseFromJson(JsonElement el)
    {
        throw new NotImplementedException();
    }
}