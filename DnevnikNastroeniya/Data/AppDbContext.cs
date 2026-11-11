using DnevnikNastroeniya.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DnevnikNastroeniya.Data
{
    internal class AppDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }

        public DbSet<Factor_Type> Factor_Types { get; set; }

        public DbSet<Forecast> Forecasts { get; set; }

        public DbSet<Mood_Scale> Mood_Scales { get; set; }

        public DbSet<Record> Records { get; set; }

        public DbSet<Tag> Tags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-PRB90QH\SQLEXPRESS;Database=DnevnikNastroeniya;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Record>()
                .HasMany(r => r.Tags)
                .WithMany(t => t.Notes)
                .UsingEntity(j => j.ToTable("RecordTags"));


            modelBuilder.Entity<Record>()
                .HasMany(r => r.Factor_Types)
                .WithMany(f => f.Notes)
                .UsingEntity(j => j.ToTable("RecordFactorTypes"));
        }
    }
}
