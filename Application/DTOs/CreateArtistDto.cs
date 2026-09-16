using System;

namespace Takwene.Application.DTOs
{
    public class CreateArtistDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }
}
