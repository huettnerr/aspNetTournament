using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ChemodartsWebApp.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ChemodartsWebApp.Data
{
    public class ChemodartsContext : DbContext
    {
        public DbSet<Player> Players { get; set; } = default!;
        public DbSet<Venue> Venues { get; set; } = default!;
        public DbSet<Tournament> Tournaments { get; set; } = default!;
        public DbSet<Round> Rounds { get; set; } = default!;
        public DbSet<Tournament> Groups { get; set; } = default!;
        public DbSet<Match> Matches { get; set; } = default!;
        public DbSet<MapperRoundsVenues> MapperRV { get; set; } = default!;
        public DbSet<MapperGroupsPlayers> MapperGP { get; set; } = default!;

        public ChemodartsContext (DbContextOptions<ChemodartsContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //var roundTypeConverter = new ValueConverter<Round.RoundType, string>(
            //    from => from.ToString(),
            //    to => (Round.RoundType)Enum.Parse(typeof(Round.RoundType), to)
            //);

            modelBuilder.Entity<Round>().Property(e => e.Type).HasConversion(createValueConverter<Round.RoundType>());  
            modelBuilder.Entity<Match>().Property(e => e.Status).HasConversion(createValueConverter<Match.MatchStatus>());  
        }

        private ValueConverter createValueConverter<T>()
        {
            return new ValueConverter<T, string>(
                    from => from.ToString(),
                    to => (T)Enum.Parse(typeof(T), to)
                );
        }
    }
}
