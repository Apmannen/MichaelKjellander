using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MichaelKjellander.Models.Wordpress;

[Table("wp_posts")]
public class WpPost
{
    [Key] [Column("ID")] public int Id { get; set; }
    [Required] [Column("post_title")] public string Title { get; set; } = "";
    [Required] [Column("post_status")] public string Status { get; set; } = "";
    [Required] [Column("post_modified")] public DateTime ModifiedTime { get; set; }
}