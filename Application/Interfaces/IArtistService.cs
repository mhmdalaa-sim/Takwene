using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Takwene.Application.DTOs;

namespace Takwene.Application.Interfaces
{
    public interface IArtistService
    {
        Task<ArtistDto> CreateAsync(CreateArtistDto dto);
        Task<List<ArtistDto>> ListAsync();
        Task<ArtistDto?> GetByIdAsync(Guid id);
        Task<ArtistDto> UpdateAsync(Guid id, CreateArtistDto dto);
        Task DeleteAsync(Guid id);
    }
}
