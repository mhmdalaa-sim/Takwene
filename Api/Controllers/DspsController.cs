using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Takwene.Domain.Entities;
using Takwene.Infrastructure.Persistence;

namespace Takwene.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DspsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public DspsController(AppDbContext db) { _db = db; }

        [HttpGet]
        public IActionResult List() => Ok(_db.Dsps.ToList());

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var d = _db.Dsps.Find(id);
            if (d == null) return NotFound();
            return Ok(d);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Dsp dto)
        {
            dto.Id = Guid.NewGuid();
            _db.Dsps.Add(dto);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Dsp dto)
        {
            var existing = await _db.Dsps.FindAsync(id);
            if (existing == null) return NotFound();
            existing.Name = dto.Name;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existing = await _db.Dsps.FindAsync(id);
            if (existing == null) return NotFound();
            _db.Dsps.Remove(existing);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
