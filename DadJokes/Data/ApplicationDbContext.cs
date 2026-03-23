using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DadJokes.Models;

namespace DadJokes.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<DadJokes.Models.Joke>? Joke { get; set; }
        public DbSet<DadJokes.Models.Vote>? Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Vote>(entity =>
            {
                entity.HasKey(v => v.Id);

                // One joke can have many votes
                entity.HasOne(v => v.Joke)
                      .WithMany(j => j.Votes)
                      .HasForeignKey(v => v.JokeId)
                      .OnDelete(DeleteBehavior.Restrict);

                // One user can have many votes
                entity.HasOne(v => v.User)
                      .WithMany()
                      .HasForeignKey(v => v.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Ensure one vote per user per joke
                entity.HasIndex(v => new { v.JokeId, v.UserId }).IsUnique();
            });
        }


    }

}