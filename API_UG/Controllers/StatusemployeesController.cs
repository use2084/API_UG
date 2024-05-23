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
    public class StatusemployeesController : ControllerBase
    {
        private readonly GidromashContext _context;

        public StatusemployeesController(GidromashContext context)
        {
            _context = context;
        }

        // GET: api/Statusemployees
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Statusemployee>), 200)]
        public async Task<IActionResult> GetStatusemployees()
        {
            var statuses = await _context.Statusemployees.ToListAsync();
            return Ok(statuses);
        }

        // GET: api/Statusemployees/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Statusemployee), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetStatusemployee(int id)
        {
            var status = await _context.Statusemployees.FindAsync(id);

            if (status == null)
            {
                return NotFound();
            }

            return Ok(status);
        }

        // POST: api/Statusemployees
        [HttpPost]
        [ProducesResponseType(typeof(Statusemployee), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateStatusemployee([FromBody] Statusemployee statusemployee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Statusemployees.Add(statusemployee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStatusemployee), new { id = statusemployee.StatusId }, statusemployee);
        }
    }
}
