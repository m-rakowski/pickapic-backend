using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PickapicBackend.Data;
using PickapicBackend.Model;

namespace PickapicBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly ILogger<ImagesController> _logger;
        private DataContext _context = null;

        public ImagesController(ILogger<ImagesController> logger, DataContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImageDTO>>> GetImages()
        {
            return await _context.Images
                .Select(x => ItemToDTO(x))
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ImageDTO>> GetImage(long id)
        {
            var image = await _context.Images.FindAsync(id);

            if (image == null)
            {
                return NotFound();
            }

            return ItemToDTO(image);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateImage(long id, ImageDTO imageDTO)
        {
            if (id != imageDTO.ImageId)
            {
                return BadRequest();
            }

            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            image.Url = imageDTO.Url;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!ImageExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ImageDTO>> CreateImage(ImageDTO imageDTO)
        {
            var image = new Image
            {
                Url = imageDTO.Url,
                PostId = imageDTO.PostId
            };

            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetImage),
                new { id = image.ImageId },
                ItemToDTO(image));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(long id)
        {
            var image = await _context.Images.FindAsync(id);

            if (image == null)
            {
                return NotFound();
            }

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImageExists(long id) =>
             _context.Images.Any(e => e.ImageId == id);

        private static ImageDTO ItemToDTO(Image image) =>
            new ImageDTO
            {
                ImageId = image.ImageId,
                Url = image.Url,
                PostId = image.PostId
            };
    }
}
