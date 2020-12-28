using Microsoft.AspNetCore.Http;

namespace PickapicBackend.Contract
{
    public class ImageUploadRequest
    {
        public IFormFile[] Files { get; set; }
    }
}
