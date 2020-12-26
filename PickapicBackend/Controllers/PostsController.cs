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
            var posts = await _context.Posts.ToListAsync();

            foreach (var post in posts)
            {
                var images = await _context.Images
                      .Where(image => image.PostId == post.PostId)
               .ToListAsync();

                post.Images = images;

                foreach (var image in images)
                {
                    var votes = await _context.Votes
                   .Where(vote => vote.ImageId == image.ImageId)
                   .ToListAsync();

                    image.Votes = votes;
                }
            }

            var postDTOs = posts.ConvertAll(post => PostToPostDTO(post));

            return Ok(postDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostDTO>> GetPost(long id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            var images = _context.Images
               .Where(image => image.PostId == id)
               .ToList();

            var imageDTOs = images.ConvertAll(image => ImageToImageDTO(image));
            return new PostDTO { AdditionDate = post.AdditionDate, PostId = post.PostId, Question = post.Question, Images = imageDTOs };
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(long id, PostDTO postDTO)
        {
            if (id != postDTO.PostId)
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

        [HttpPost("posts")]
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
                new { id = post.PostId },
                PostToPostDTO(post));
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
             _context.Posts.Any(e => e.PostId == id);

        private static PostDTO PostToPostDTO(Post post) =>
            new PostDTO
            {
                PostId = post.PostId,
                Question = post.Question,
                Images = post.Images.ConvertAll(image => ImageToImageDTO(image))
            };


        private static ImageDTO ImageToImageDTO(Image image) =>
            new ImageDTO
            {
                ImageId = image.ImageId,
                Url = image.Url,
                Votes = image.Votes.ConvertAll(vote => VoteToVoteDTO(vote))
            };


        private static VoteDTO VoteToVoteDTO(Vote vote) => new VoteDTO { VoteId = vote.VoteId };

    }
}
