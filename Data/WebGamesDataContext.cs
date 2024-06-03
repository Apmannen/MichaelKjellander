﻿using MichaelKjellander.Models.WebGames;
using MichaelKjellander.SharedUtils;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace MichaelKjellander.Data;

public class WebGamesDataContext : DbContext
{
    public DbSet<Word> Words { get; set; }
    public DbSet<WordGuessGameProgress> WordGuessGameProgresses { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ServerVersion serverVersion = ServerVersion.Create(8, 4, 0, ServerType.MySql);
        string connectionString = EnvironmentUtil.Get(EnvVariable.SG_MYSQLCONNSTRING);
        optionsBuilder.UseMySql(
            connectionString: connectionString, 
            serverVersion: serverVersion,
            mySqlOptionsAction: null);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
        modelBuilder.Entity<Word>()
            .HasMany(w => w.GuessGameProgresses)
            .WithOne(p => p.Word)
            .HasForeignKey(p => p.WordId)
            .HasPrincipalKey(w => w.Id);
    }
}