using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_UG.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace API_UG.Controllers
{
    public class ServicesController : ControllerBase
    {
        private readonly GidromashContext _context;

        public ServicesController(GidromashContext context)
        {
            _context = context;
        }

        // GET: api/Services
        [HttpGet("services")]
        [ProducesResponseType(typeof(IEnumerable<Service>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetServices()
        {
            var services = await _context.Services.ToListAsync();
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };

            string responseData = JsonSerializer.Serialize(services, options);

            return Ok(responseData);
        }

        // GET: api/Services/5
        [HttpGet("service/{id}")]
        [ProducesResponseType(typeof(Service), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetService(int id)
        {
            var service = await _context.Services.FirstOrDefaultAsync(m => m.ServiceId == id);

            if (service == null)
            {
                return NotFound();
            }

            return Ok(service);
        }

        // POST: api/Services
        [HttpPost]
        [ProducesResponseType(typeof(Service), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateService([FromBody] Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetService), new { id = service.ServiceId }, service);
        }
    }
}
