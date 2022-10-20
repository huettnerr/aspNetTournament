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
        //public DbSet<Score> Scores { get; set; } = default!;
        public DbSet<MapRoundVenue> MapperRV { get; set; } = default!;
        public DbSet<MapTournamentPlayer> MapperTP { get; set; } = default!;

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
            modelBuilder.Entity<Match>().HasOne<Player>(m => m.Player1).WithMany(p => p.MatchesHome).HasForeignKey(m => m.Player1Id);
            modelBuilder.Entity<Match>().HasOne<Player>(m => m.Player2).WithMany(p => p.MatchesAway).HasForeignKey(m => m.Player2Id);
            modelBuilder.Entity<Match>().HasOne<Score>(m => m.Score).WithOne(s => s.Match).HasForeignKey<Score>(s => s.MatchId);
            modelBuilder.Entity<Venue>().HasOne<Match>(v => v.Match).WithOne(m => m.Venue).HasForeignKey<Venue>(v => v.MatchId);

            //Navigation Entries of mapped tables
            modelBuilder.Entity<MapTournamentPlayer>().HasOne<Tournament>(map => map.Tournament).WithMany(p => p.MappedPlayers).HasForeignKey(map => map.TPM_TournamentId);
            modelBuilder.Entity<MapTournamentPlayer>().HasOne<Player>(map => map.Player).WithMany(p => p.MappedTournaments).HasForeignKey(map => map.TPM_PlayerId);
            modelBuilder.Entity<MapTournamentPlayer>().HasOne<Seed>(map => map.Seed).WithOne(s => s.MappedTournamentPlayer).HasForeignKey<MapTournamentPlayer>(map => map.TPM_SeedId);

            modelBuilder.Entity<MapRoundVenue>().HasOne<Round>(map => map.Round).WithMany(r => r.MappedVenues).HasForeignKey(map => map.RVM_RoundId);
            modelBuilder.Entity<MapRoundVenue>().HasOne<Venue>(map => map.Venue).WithMany(p => p.MappedRounds).HasForeignKey(map => map.RVM_VenueId);

            //Converter for enums
            modelBuilder.Entity<Round>().Property(e => e.Modus).HasConversion(createValueConverter<Round.RoundModus>());  
            modelBuilder.Entity<Round>().Property(e => e.Scoring).HasConversion(createValueConverter<Round.ScoreType>());  
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
