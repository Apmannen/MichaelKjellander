using System.ComponentModel.DataAnnotations;

namespace MichaelKjellander.Domains.Models;

public abstract class DbModel
{
    protected const int VarcharLength = 256;
    
    [Key]
    [Required]
    public int Id { get; set; }
}