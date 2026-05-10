using BookService.Api.DTOs;
using BookService.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaUnitsController : ControllerBase
    {
        private readonly IMediaUnitService _service;

        public MediaUnitsController(IMediaUnitService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var mediaUnits = await _service.GetAllAsync();
            return Ok(mediaUnits);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var mediaUnit = await _service.GetByIdAsync(id);
            if (mediaUnit == null)
                return NotFound();

            return Ok(mediaUnit);
        }


        [HttpPost]
        public async Task<IActionResult> Create(MediaUnitCreateDTO dto)
        {

            var created = await _service.CreateAsync(dto);
            if (created == null)
                return NotFound($"MediaItem with ID {dto.MediaItemId}) not found.");

            return Ok(created);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MediaUnitUpdateDTO dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            if (!success)
                return NotFound();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}