using System;

namespace Takwene.Domain.Entities
{
    public enum DistributionStatus
    {
        Pending,
        Live,
        Rejected
    }

    public class TrackDistribution
    {
        public Guid Id { get; set; }

        public Guid TrackId { get; set; }
        public Track? Track { get; set; }

        public Guid DspId { get; set; }
        public Dsp? Dsp { get; set; }

        public DateTime SubmittedAt { get; set; }
        public DistributionStatus Status { get; set; } = DistributionStatus.Pending;
    }
}
