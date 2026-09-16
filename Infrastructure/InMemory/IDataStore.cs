using System.Collections.Generic;
using Takwene.Domain.Entities;

namespace Takwene.Infrastructure.InMemory
{
    public interface IDataStore
    {
        List<Artist> Artists { get; }
        List<Track> Tracks { get; }
        List<Dsp> Dsps { get; }
        List<TrackDistribution> TrackDistributions { get; }
    }
}
