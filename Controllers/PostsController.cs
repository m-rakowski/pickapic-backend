using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PickapicBackend.Data;
using PickapicBackend.Model;

namespace PickapicBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
   private readonly ILogger<PostsController> _logger;
        private DataContext _context = null;

        public PostsController(ILogger<PostsController> logger, DataContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult GetPosts()
        {
            return Ok(_context.Posts.ToList());
        }
    }
}
