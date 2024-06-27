using MichaelKjellander.Domains.Models.WebGames;
using Microsoft.EntityFrameworkCore;

namespace MichaelKjellander.Data;

public class WebGamesDataContext : DataContext
{
    public DbSet<Word> Words { get; set; }
    public DbSet<WordGuessGameProgress> WordGuessGameProgresses { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        if (IsCalledByPopulateScript)
        {
            List<Word> words = [];
            string path = "Files/ord.niklas.frykholm.txt";
            using FileStream stream = File.OpenRead(path);
            using StreamReader reader = new StreamReader(stream);
            string? line;
            int id = 1;
            while ((line = reader.ReadLine()) != null)
            {
                words.Add(new Word{Id = id++, WordString = line});
            }
            modelBuilder.Entity<Word>().HasData(words);
        }
        
        modelBuilder.Entity<Word>()
            .HasMany(w => w.GuessGameProgresses)
            .WithOne(p => p.Word)
            .HasForeignKey(p => p.WordId)
            .HasPrincipalKey(w => w.Id);
        
        modelBuilder.Entity<WordGuessGameProgress>()
            .HasIndex(wggp => new { wggp.Uuid }).IsUnique();
    }
}