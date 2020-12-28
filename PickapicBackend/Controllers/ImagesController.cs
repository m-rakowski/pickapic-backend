using Microsoft.AspNetCore.Mvc;
using PickapicBackend.Contract;
using PickapicBackend.Data;
using PickapicBackend.Model;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
namespace PickapicBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ImagesController : ControllerBase
    {
        private DataContext _context = null;

        public ImagesController(DataContext _context)
        {
            this._context = _context;
        }

        [ActionName("image")]
        [HttpGet]
        public async Task<IActionResult> GetImage([FromQuery] long id)
        {
            var image = await _context.Images.FindAsync(id).ConfigureAwait(true);
            if (image is null)
                return NotFound();

            using var stream = System.IO.File.OpenRead(image.ImagePath);
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream).ConfigureAwait(true);
            var bytes = memoryStream.ToArray();
            return File(bytes, image.MimeType);
        }

        [ActionName("image")]
        [HttpPost]
        public async Task<ActionResult<ImageUploadResult>> PostImage([FromForm] ImageUploadRequest imageUploadRequest)
        {

            if ((imageUploadRequest is null) || (imageUploadRequest.Files is null))
                return NotFound("No files provided");

            var files = imageUploadRequest.Files;

            long maxId = _context.Images
              .OrderByDescending(p => p.ImageId)
              .Select(p => p.ImageId)
              .FirstOrDefault();

            var root = Directory.GetCurrentDirectory();
            var cmsPath = Path.Combine(root, "cms");
            Directory.CreateDirectory(cmsPath);

            var result = new ImageUploadResult();

            for(int i = 0; i < files.Length; i++)
            {
                string fileName = (maxId + 1 + i).ToString();
                string filePath = Path.Combine(cmsPath, fileName);

                using var stream = files[i].OpenReadStream();
                using var fileStream = System.IO.File.OpenWrite(filePath);
                await stream.CopyToAsync(fileStream).ConfigureAwait(true);

                var image = new Image
                {
                    PostId = 1,
                    ImagePath = filePath,
                    MimeType = files[i].ContentType
                };

                _context.Images.Add(image);
                await _context.SaveChangesAsync().ConfigureAwait(true);

                result.Ids.Add(fileName);

            }

            return Ok(result);
        }
    }
}
