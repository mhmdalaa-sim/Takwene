using System.Collections.Generic;
using System.Threading.Tasks;
using Takwene.Application.DTOs;

namespace Takwene.Application.Interfaces
{
    public interface IArtistService
    {
        Task<ArtistDto> CreateAsync(CreateArtistDto dto);
        Task<List<ArtistDto>> ListAsync();
    }
}
