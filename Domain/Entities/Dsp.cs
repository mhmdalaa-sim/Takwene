using System;
using System.Collections.Generic;

namespace Takwene.Domain.Entities
{
    public class Dsp
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<TrackDistribution> TrackDistributions { get; set; } = new List<TrackDistribution>();
    }
}
