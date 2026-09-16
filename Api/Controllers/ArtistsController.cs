using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Takwene.Application.DTOs;
using Takwene.Application.Interfaces;

namespace Takwene.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistService _service;
        public ArtistsController(IArtistService service) { _service = service; }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateArtistDto dto)
        {
            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(List), new { id = created.Id }, created);
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var list = await _service.ListAsync();
            return Ok(list);
        }
    }
}
