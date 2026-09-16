using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Takwene.Infrastructure.Persistence;
using Takwene.Domain.Entities;
using Takwene.Application.DTOs;

namespace Takwene.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrackDistributionsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public TrackDistributionsController(AppDbContext db) { _db = db; }

        [HttpGet]
        public IActionResult List() => Ok(_db.TrackDistributions.Select(d => new {
            d.Id, d.TrackId, d.DspId, d.SubmittedAt, d.Status
        }).ToList());

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var d = _db.TrackDistributions.Find(id);
            if (d == null) return NotFound();
            return Ok(d);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateDistributionStatusDto dto)
        {
            var d = await _db.TrackDistributions.FindAsync(id);
            if (d == null) return NotFound();
            if (!Enum.TryParse<DistributionStatus>(dto.Status, true, out var st)) return BadRequest(new { error = "Invalid status" });
            d.Status = st;
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
