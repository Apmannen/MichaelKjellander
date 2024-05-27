using System.ComponentModel.DataAnnotations.Schema;

namespace MichaelKjellander.Models.Wordpress;

public class WpTermTaxonomy
{
    [Column("term_taxonomy_id")]
    public int TermTaxonomyId { get; set; }
    [Column("term_id")]
    public int TermId { get; set; }
    
    public virtual ICollection<WpTermRelationship> TermRelationships { get; set; }
}