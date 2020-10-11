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
        public async Task<ActionResult<IEnumerable<PostDTO>>> GetPosts()
        {
            return await _context.Posts
                .Select(x => ItemToDTO(x))
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostDTO>> GetPost(long id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return ItemToDTO(post);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(long id, PostDTO postDTO)
        {
            if (id != postDTO.Id)
            {
                return BadRequest();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            post.Question = postDTO.Question;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!PostExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<PostDTO>> CreatePost(PostDTO postDTO)
        {
            var post = new Post
            {
                Question = postDTO.Question
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetPost),
                new { id = post.Id },
                ItemToDTO(post));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(long id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostExists(long id) =>
             _context.Posts.Any(e => e.Id == id);

        private static PostDTO ItemToDTO(Post post) =>
            new PostDTO
            {
                Id = post.Id,
                Question = post.Question
            };
    }
}
