using System;
using System.Collections.Generic;
using Takwene.Domain.Entities;

namespace Takwene.Application.DTOs
{
    public class TrackDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public Guid ArtistId { get; set; }
        public string ArtistName { get; set; } = string.Empty;
        public string Isrc { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; } = string.Empty;
        public TrackStatus Status { get; set; }
        public List<TrackDistributionDto> Distributions { get; set; } = new List<TrackDistributionDto>();
    }
}
