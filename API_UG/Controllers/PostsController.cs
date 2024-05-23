using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_UG.Models;

namespace API_UG.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly GidromashContext _context;

        public PostsController(GidromashContext context)
        {
            _context = context;
        }

        // GET: api/Posts
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Post>), 200)]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _context.Posts.ToListAsync();
            return Ok(posts);
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Post), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        // POST: api/Posts
        [HttpPost]
        [ProducesResponseType(typeof(Post), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreatePost([FromBody] Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPost), new { id = post.PostId }, post);
        }

    }
}
