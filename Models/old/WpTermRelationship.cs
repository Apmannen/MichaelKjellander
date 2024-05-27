using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MichaelKjellander.Models.Wordpress;

[Table("wp_term_relationships")]
public class WpTermRelationship
{
    public virtual WpPost Post { get; set; }
    [Column("object_id")]
    public int ObjectId { get; set; }
    [Column("term_taxonomy_id")]
    public int TermTaxonomyId { get; set; }
}