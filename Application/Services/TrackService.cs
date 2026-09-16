using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Takwene.Application.DTOs;
using Takwene.Application.Interfaces;
using Takwene.Domain.Entities;
using Takwene.Infrastructure.Persistence;

namespace Takwene.Application.Services
{
    public class TrackService : ITrackService
    {
        private readonly AppDbContext _db;
        public TrackService(AppDbContext db) { _db = db; }

        public async Task<TrackDto> CreateAsync(CreateTrackDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title)) throw new ArgumentException("Title is required");
            if (string.IsNullOrWhiteSpace(dto.Isrc)) throw new ArgumentException("ISRC is required");

            var exists = await _db.Tracks.AnyAsync(t => t.Isrc == dto.Isrc);
            if (exists) throw new ArgumentException("ISRC must be unique");

            var track = new Track
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                ArtistId = dto.ArtistId,
                Isrc = dto.Isrc,
                ReleaseDate = dto.ReleaseDate,
                Genre = dto.Genre,
                Status = TrackStatus.Draft
            };

            _db.Tracks.Add(track);
            await _db.SaveChangesAsync();

            var artist = await _db.Artists.FindAsync(track.ArtistId);

            return new TrackDto
            {
                Id = track.Id,
                Title = track.Title,
                ArtistId = track.ArtistId,
                ArtistName = artist?.Name ?? string.Empty,
                Isrc = track.Isrc,
                ReleaseDate = track.ReleaseDate,
                Genre = track.Genre,
                Status = track.Status
            };
        }

        public async Task<List<TrackDto>> ListAsync(Guid? artistId, string? genre, string? status)
        {
            var q = _db.Tracks.Include(t => t.Artist).AsQueryable();
            if (artistId.HasValue) q = q.Where(t => t.ArtistId == artistId.Value);
            if (!string.IsNullOrWhiteSpace(genre)) q = q.Where(t => t.Genre == genre);
            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<TrackStatus>(status, true, out var st)) q = q.Where(t => t.Status == st);

            return await q.Select(t => new TrackDto
            {
                Id = t.Id,
                Title = t.Title,
                ArtistId = t.ArtistId,
                ArtistName = t.Artist != null ? t.Artist.Name : string.Empty,
                Isrc = t.Isrc,
                ReleaseDate = t.ReleaseDate,
                Genre = t.Genre,
                Status = t.Status
            }).ToListAsync();
        }

        public async Task<TrackDto?> GetByIdAsync(Guid id)
        {
            var t = await _db.Tracks
                .Include(x => x.Artist)
                .Include(x => x.Distributions)!.ThenInclude(d => d.Dsp)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (t == null) return null;

            var dto = new TrackDto
            {
                Id = t.Id,
                Title = t.Title,
                ArtistId = t.ArtistId,
                ArtistName = t.Artist?.Name ?? string.Empty,
                Isrc = t.Isrc,
                ReleaseDate = t.ReleaseDate,
                Genre = t.Genre,
                Status = t.Status,
                Distributions = t.Distributions.Select(d => new TrackDistributionDto
                {
                    Id = d.Id,
                    DspId = d.DspId,
                    DspName = d.Dsp?.Name ?? string.Empty,
                    SubmittedAt = d.SubmittedAt,
                    Status = d.Status
                }).ToList()
            };

            return dto;
        }

        public async Task DistributeAsync(Guid trackId, Guid[] dspIds)
        {
            var track = await _db.Tracks.FindAsync(trackId);
            if (track == null) throw new ArgumentException("Track not found");

            foreach (var dspId in dspIds)
            {
                var exists = await _db.TrackDistributions.AnyAsync(td => td.TrackId == trackId && td.DspId == dspId);
                if (exists) continue;
                var td = new TrackDistribution
                {
                    Id = Guid.NewGuid(),
                    TrackId = trackId,
                    DspId = dspId,
                    SubmittedAt = DateTime.UtcNow,
                    Status = DistributionStatus.Pending
                };
                _db.TrackDistributions.Add(td);
            }

            // Mark track as submitted if it was draft
            if (track.Status == TrackStatus.Draft) track.Status = TrackStatus.Submitted;

            await _db.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(Guid trackId, string status)
        {
            var track = await _db.Tracks.FindAsync(trackId);
            if (track == null) throw new ArgumentException("Track not found");
            if (!Enum.TryParse<TrackStatus>(status, true, out var st)) throw new ArgumentException("Invalid status");
            track.Status = st;
            await _db.SaveChangesAsync();
        }
    }
}
