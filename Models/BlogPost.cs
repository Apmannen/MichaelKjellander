using System.ComponentModel.DataAnnotations;

namespace MichaelKjellander.Models;

public class BlogPost
{
    [Key]
    public int Id { get; set; }

    [Required] public string Title { get; set; } = "";
}
