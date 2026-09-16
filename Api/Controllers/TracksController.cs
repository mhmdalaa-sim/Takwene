using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Takwene.Application.DTOs;
using Takwene.Application.Interfaces;

namespace Takwene.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TracksController : ControllerBase
    {
        private readonly ITrackService _service;
        public TracksController(ITrackService service) { _service = service; }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTrackDto dto)
        {
            try
            {
                var t = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = t.Id }, t);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] Guid? artistId, [FromQuery] string? genre, [FromQuery] string? status)
        {
            var list = await _service.ListAsync(artistId, genre, status);
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var t = await _service.GetByIdAsync(id);
            if (t == null) return NotFound();
            return Ok(t);
        }

        // Protected endpoint: requires JWT
        [HttpPost("{id}/distribute")]
        [Authorize]
        public async Task<IActionResult> Distribute(Guid id, [FromBody] Guid[] dspIds)
        {
            try
            {
                await _service.DistributeAsync(id, dspIds);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusDto dto)
        {
            try
            {
                await _service.UpdateStatusAsync(id, dto.Status);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTrackDto dto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, dto);
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
