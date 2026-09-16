using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Takwene.Application.DTOs;

namespace Takwene.Application.Interfaces
{
    public interface ITrackService
    {
        Task<TrackDto> CreateAsync(CreateTrackDto dto);
        Task<List<TrackDto>> ListAsync(Guid? artistId, string? genre, string? status);
        Task<TrackDto?> GetByIdAsync(Guid id);
        Task DistributeAsync(Guid trackId, Guid[] dspIds);
        Task UpdateStatusAsync(Guid trackId, string status);
        Task<TrackDto> UpdateAsync(Guid trackId, UpdateTrackDto dto);
        Task DeleteAsync(Guid trackId);
    }
}
