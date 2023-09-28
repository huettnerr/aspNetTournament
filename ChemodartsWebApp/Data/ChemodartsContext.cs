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
        public DbSet<Group> Groups { get; set; } = default!;
        public DbSet<Seed> Seeds { get; set; } = default!;
        public DbSet<Match> Matches { get; set; } = default!;
        public DbSet<Score> Scores { get; set; } = default!;
        public DbSet<MapRoundVenue> MapperRV { get; set; } = default!;
        public DbSet<MapRoundSeedPlayer> MapperRP { get; set; } = default!;

        public ChemodartsContext (DbContextOptions<ChemodartsContext> options)
            : base(options)
        {
            
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    this.Configuration.ProxyCreationEnabled = true;
        //    DbContext.Configuration.LazyLoadingEnabled = true;
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Navigation for Database
            modelBuilder.Entity<Tournament>().HasMany<Round>(t => t.Rounds).WithOne(r => r.Tournament).HasForeignKey(r => r.TournamentId);
            modelBuilder.Entity<Round>().HasMany<Group>(r => r.Groups).WithOne(g => g.Round).HasForeignKey(g => g.RoundId);
            modelBuilder.Entity<Group>().HasMany<Match>(g => g.Matches).WithOne(m => m.Group).HasForeignKey(m => m.GroupId);
            modelBuilder.Entity<Group>().HasMany<Seed>(g => g.Seeds).WithOne(s => s.Group).HasForeignKey(s => s.GroupId);
            modelBuilder.Entity<Match>().HasOne<Seed>(m => m.Seed1).WithMany(p => p.MatchesAsS1).HasForeignKey(m => m.Seed1Id);
            modelBuilder.Entity<Match>().HasOne<Seed>(m => m.Seed2).WithMany(p => p.MatchesAsS2).HasForeignKey(m => m.Seed2Id);
            modelBuilder.Entity<Match>().HasOne<Score>(m => m.Score).WithOne(s => s.Match).HasForeignKey<Score>(s => s.MatchId);
            modelBuilder.Entity<Match>().HasOne<Venue>(v => v.Venue).WithOne(m => m.Match).HasForeignKey<Match>(v => v.VenueId);
            modelBuilder.Entity<Seed>().HasOne<Match>(s => s.AncestorMatch).WithOne(m => m.WinnerSeedFollowUp).HasForeignKey<Seed>(s => s.AncestorMatchId);
            modelBuilder.Entity<Seed>().HasOne<SeedStatistics>(s => s.SeedStatistics).WithOne(ss => ss.Seed).HasForeignKey<SeedStatistics>(ss => ss.SeedId);

            //Navigation Entries of mapped tables

            modelBuilder.Entity<MapRoundSeedPlayer>().HasOne<Round>(map => map.Round).WithMany(r => r.MappedSeedsPlayers).HasForeignKey(map => map.TSP_RoundId);
            modelBuilder.Entity<MapRoundSeedPlayer>().HasOne<Player>(map => map.Player).WithMany(p => p.MappedTournaments).HasForeignKey(map => map.TSP_PlayerId);
            modelBuilder.Entity<MapRoundSeedPlayer>().HasOne<Seed>(map => map.Seed).WithOne(s => s.MappedRoundSeedPlayer).HasForeignKey<MapRoundSeedPlayer>(map => map.TSP_SeedId);

            modelBuilder.Entity<MapRoundVenue>().HasOne<Round>(map => map.Round).WithMany(r => r.MappedVenues).HasForeignKey(map => map.RVM_RoundId);
            modelBuilder.Entity<MapRoundVenue>().HasOne<Venue>(map => map.Venue).WithMany(p => p.MappedRounds).HasForeignKey(map => map.RVM_VenueId);

            //Converter for enums
            modelBuilder.Entity<Round>().Property(e => e.Modus).HasConversion(createValueConverter<RoundModus>());  
            modelBuilder.Entity<Round>().Property(e => e.Scoring).HasConversion(createValueConverter<ScoreType>());  
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
