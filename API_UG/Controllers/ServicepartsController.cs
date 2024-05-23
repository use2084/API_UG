using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_UG.Models;

namespace API_UG.Controllers
{
    public class ServicepartsController : ControllerBase
    {
        private readonly GidromashContext _context;

        public ServicepartsController(GidromashContext context)
        {
            _context = context;
        }

        // GET: api/Serviceparts
        [HttpGet("serviceparts")]
        [ProducesResponseType(typeof(IEnumerable<Servicepart>), 200)]
        public async Task<IActionResult> GetServiceparts()
        {
            var serviceparts = await _context.Serviceparts
                .Include(s => s.Agreement)
                .Include(s => s.Order)
                .Include(s => s.Service)
                .ToListAsync();

            return Ok(serviceparts);
        }

        // GET: api/Serviceparts/5
        [HttpGet("serviceparts/{id}")]
        [ProducesResponseType(typeof(Servicepart), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetServicePart(int id)
        {
            var servicePart = await _context.Serviceparts.FindAsync(id);
            if (servicePart == null)
            {
                return NotFound();
            }
            return Ok(servicePart);
        }

        [HttpPost("servicepartspost")]
        [ProducesResponseType(typeof(Servicepart), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddServicePart([FromBody] Servicepart servicePart)
        {
            if (servicePart == null)
            {
                return BadRequest("Объект табличной части для услуги пуст.");
            }

            _context.Serviceparts.Add(servicePart);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServicePart), new { id = servicePart.ServicePartId }, servicePart);
        }
    }
}

