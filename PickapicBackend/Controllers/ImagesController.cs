using System;
using System.Collections;
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
        public async Task<ActionResult<IEnumerable<Image>>> GetImages()
        {
            var images = await _context.Images.ToListAsync();
            foreach (var image in images)
            {
                var votes = await _context.Votes
               .Where(vote => vote.ImageId == image.ImageId)
               .ToListAsync();

                image.Votes = votes;       
            }

            var imageDTOs = images.ConvertAll(image => ImageToImageDTO(image));
            return Ok(imageDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ImageDTO>> GetImage(long id)
        {
            var post = await _context.Images.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(ImageToImageDTO(post));
        }
        private static ImageDTO ImageToImageDTO(Image image) => new ImageDTO {
            ImageId = image.ImageId,
            Url = image.Url,
            Votes = image.Votes.ConvertAll(vote => new VoteDTO { VoteId = vote.VoteId })

        };
    }
}
