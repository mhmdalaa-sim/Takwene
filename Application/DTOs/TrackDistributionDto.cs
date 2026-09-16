using System;
using Takwene.Domain.Entities;

namespace Takwene.Application.DTOs
{
    public class TrackDistributionDto
    {
        public Guid Id { get; set; }
        public Guid DspId { get; set; }
        public string DspName { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
        public DistributionStatus Status { get; set; }
    }
}
