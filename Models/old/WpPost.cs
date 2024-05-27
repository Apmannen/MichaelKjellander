using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MichaelKjellander.Models.Wordpress;

/*
 * mysql> SELECT * FROM wp_term_relationships tr
 * LEFT JOIN wp_term_taxonomy tt ON tt.term_taxonomy_id=tr.term_taxonomy_id
 * LEFT JOIN wp_terms t ON t.term_id=tt.term_id
 * WHERE tr.object_id=1006 \G
 */

[Table("wp_posts")]
public class WpPost
{
    [Key] [Column("ID")] public int Id { get; set; }
    [Required] [Column("post_title")] public string Title { get; set; } = "";
    [Required] [Column("post_status")] public string Status { get; set; } = "";
    [Required] [Column("post_date")] public DateTime? PublishTime { get; set; }
    [Required] [Column("post_modified")] public DateTime? ModifiedTime { get; set; }
    [Required] [Column("post_content")] public string Content { get; set; } = "";

    
    //public ICollection<WpTermRelationship> TermRelationship { get; set; }
    public virtual List<WpTermRelationship> TermRelationships { get; set; }
}