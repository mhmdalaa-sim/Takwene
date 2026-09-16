using System;
using System.Collections.Generic;

namespace Takwene.Application.DTOs
{
    public class CreateTrackDto
    {
        public string Title { get; set; } = string.Empty;
        public Guid ArtistId { get; set; }
        public string Isrc { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; } = string.Empty;
    }
}
