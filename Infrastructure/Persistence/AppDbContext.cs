using Microsoft.EntityFrameworkCore;
using Takwene.Domain.Entities;
using System;

namespace Takwene.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Artist> Artists { get; set; } = null!;
        public DbSet<Track> Tracks { get; set; } = null!;
        public DbSet<Dsp> Dsps { get; set; } = null!;
        public DbSet<TrackDistribution> TrackDistributions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Track>()
                .HasIndex(t => t.Isrc)
                .IsUnique();

            // Seed data
            var artist1 = new Artist { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Luna Ray", Email = "luna.ray@example.com", Country = "US" };
            var artist2 = new Artist { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Musa K.", Email = "musa.k@example.com", Country = "NG" };
            var artist3 = new Artist { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "The Echoes", Email = "echoes@example.com", Country = "GB" };

            var dsp1 = new Dsp { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "Spotify" };
            var dsp2 = new Dsp { Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), Name = "Apple Music" };
            var dsp3 = new Dsp { Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), Name = "YouTube" };

            var track1 = new Track { Id = Guid.Parse("10000000-0000-0000-0000-000000000001"), Title = "Midnight Drive", ArtistId = artist1.Id, Isrc = "US-R1L-20-00001", ReleaseDate = new DateTime(2023, 6, 1), Genre = "Synthwave", Status = TrackStatus.Draft };
            var track2 = new Track { Id = Guid.Parse("10000000-0000-0000-0000-000000000002"), Title = "Ocean Eyes", ArtistId = artist1.Id, Isrc = "US-R1L-20-00002", ReleaseDate = new DateTime(2022, 11, 11), Genre = "Indie Pop", Status = TrackStatus.Submitted };
            var track3 = new Track { Id = Guid.Parse("10000000-0000-0000-0000-000000000003"), Title = "Sunrise Over Lagos", ArtistId = artist2.Id, Isrc = "NG-MK-21-00003", ReleaseDate = new DateTime(2024, 1, 5), Genre = "Afrobeats", Status = TrackStatus.Distributed };
            var track4 = new Track { Id = Guid.Parse("10000000-0000-0000-0000-000000000004"), Title = "City Lights", ArtistId = artist3.Id, Isrc = "GB-TE-22-00004", ReleaseDate = new DateTime(2021, 8, 20), Genre = "Rock", Status = TrackStatus.Submitted };
            var track5 = new Track { Id = Guid.Parse("10000000-0000-0000-0000-000000000005"), Title = "Desert Winds", ArtistId = artist2.Id, Isrc = "NG-MK-21-00005", ReleaseDate = new DateTime(2020, 3, 15), Genre = "World", Status = TrackStatus.Draft };
            var track6 = new Track { Id = Guid.Parse("10000000-0000-0000-0000-000000000006"), Title = "Neon Nights", ArtistId = artist3.Id, Isrc = "GB-TE-22-00006", ReleaseDate = new DateTime(2019, 12, 12), Genre = "Electronic", Status = TrackStatus.Distributed };
            var track7 = new Track { Id = Guid.Parse("10000000-0000-0000-0000-000000000007"), Title = "Forest Hymn", ArtistId = artist1.Id, Isrc = "US-R1L-20-00007", ReleaseDate = new DateTime(2024, 5, 2), Genre = "Ambient", Status = TrackStatus.Draft };
            var track8 = new Track { Id = Guid.Parse("10000000-0000-0000-0000-000000000008"), Title = "Paper Planes", ArtistId = artist3.Id, Isrc = "GB-TE-22-00008", ReleaseDate = new DateTime(2022, 2, 2), Genre = "Pop", Status = TrackStatus.Submitted };

            var dist1 = new TrackDistribution { Id = Guid.Parse("20000000-0000-0000-0000-000000000001"), TrackId = track3.Id, DspId = dsp1.Id, SubmittedAt = new DateTime(2024, 1, 10), Status = DistributionStatus.Live };
            var dist2 = new TrackDistribution { Id = Guid.Parse("20000000-0000-0000-0000-000000000002"), TrackId = track3.Id, DspId = dsp2.Id, SubmittedAt = new DateTime(2024, 1, 12), Status = DistributionStatus.Live };
            var dist3 = new TrackDistribution { Id = Guid.Parse("20000000-0000-0000-0000-000000000003"), TrackId = track6.Id, DspId = dsp3.Id, SubmittedAt = new DateTime(2020, 1, 5), Status = DistributionStatus.Live };
            var dist4 = new TrackDistribution { Id = Guid.Parse("20000000-0000-0000-0000-000000000004"), TrackId = track2.Id, DspId = dsp1.Id, SubmittedAt = new DateTime(2022, 11, 20), Status = DistributionStatus.Pending };

            modelBuilder.Entity<Artist>().HasData(artist1, artist2, artist3);
            modelBuilder.Entity<Dsp>().HasData(dsp1, dsp2, dsp3);
            modelBuilder.Entity<Track>().HasData(
                track1, track2, track3, track4, track5, track6, track7, track8
            );
            modelBuilder.Entity<TrackDistribution>().HasData(dist1, dist2, dist3, dist4);
        }
    }
}
