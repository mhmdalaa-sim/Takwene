using System;
using System.Collections.Generic;

namespace Takwene.Domain.Entities
{
    public enum TrackStatus
    {
        Draft,
        Submitted,
        Distributed
    }

    public class Track
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public Guid ArtistId { get; set; }
        public Artist? Artist { get; set; }

        public string Isrc { get; set; } = string.Empty; // unique
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; } = string.Empty;
        public TrackStatus Status { get; set; } = TrackStatus.Draft;

        public ICollection<TrackDistribution> Distributions { get; set; } = new List<TrackDistribution>();
    }
}
