using System.ComponentModel.DataAnnotations;

namespace MichaelKjellander.Models;

public abstract class DbModel
{
    [Key]
    [Required]
    public int? Id { get; set; }
}