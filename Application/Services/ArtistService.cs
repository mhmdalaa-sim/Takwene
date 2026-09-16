using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Takwene.Application.DTOs;
using Takwene.Application.Interfaces;
using Takwene.Infrastructure.Persistence;
using Takwene.Domain.Entities;
using System;

namespace Takwene.Application.Services
{
    public class ArtistService : IArtistService
    {
        private readonly AppDbContext _db;
        public ArtistService(AppDbContext db) { _db = db; }

        public async Task<ArtistDto> CreateAsync(CreateArtistDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) throw new ArgumentException("Name is required");
            var artist = new Artist { Id = Guid.NewGuid(), Name = dto.Name, Email = dto.Email, Country = dto.Country };
            _db.Artists.Add(artist);
            await _db.SaveChangesAsync();
            return new ArtistDto { Id = artist.Id, Name = artist.Name, Email = artist.Email, Country = artist.Country };
        }

        public async Task<List<ArtistDto>> ListAsync()
        {
            return await _db.Artists
                .Select(a => new ArtistDto { Id = a.Id, Name = a.Name, Email = a.Email, Country = a.Country })
                .ToListAsync();
        }

        public async Task<ArtistDto?> GetByIdAsync(Guid id)
        {
            var a = await _db.Artists.FindAsync(id);
            if (a == null) return null;
            return new ArtistDto { Id = a.Id, Name = a.Name, Email = a.Email, Country = a.Country };
        }

        public async Task<ArtistDto> UpdateAsync(Guid id, CreateArtistDto dto)
        {
            var a = await _db.Artists.FindAsync(id);
            if (a == null) throw new ArgumentException("Artist not found");
            a.Name = dto.Name;
            a.Email = dto.Email;
            a.Country = dto.Country;
            await _db.SaveChangesAsync();
            return new ArtistDto { Id = a.Id, Name = a.Name, Email = a.Email, Country = a.Country };
        }

        public async Task DeleteAsync(Guid id)
        {
            var a = await _db.Artists.FindAsync(id);
            if (a == null) throw new ArgumentException("Artist not found");
            _db.Artists.Remove(a);
            await _db.SaveChangesAsync();
        }
    }
}
