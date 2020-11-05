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
    public class VotesController : ControllerBase
    {
        private readonly ILogger<PostsController> _logger;
        private DataContext _context = null;

        public VotesController(ILogger<PostsController> logger, DataContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet ]
        public async Task<ActionResult<IEnumerable<Vote>>> GetVotes()
        {
            var votes = await _context.Votes.ToListAsync();
            var voteDTOs = votes.ConvertAll(vote => new VoteDTO
            {
                VoteId = vote.VoteId 
            });
            return Ok(voteDTOs); 
        } 
    }
}
