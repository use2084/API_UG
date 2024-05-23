using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_UG.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace API_UG.Controllers
{
    public class ProductpartsController : ControllerBase
    {
        private readonly GidromashContext _context;

        public ProductpartsController(GidromashContext context)
        {
            _context = context;
        }

        // GET: api/Productparts
        [HttpGet("productparts")]
        [ProducesResponseType(typeof(IEnumerable<Productpart>), 200)]
        public async Task<IActionResult> GetProductparts()
        {
            var productparts = await _context.Productparts
                .Include(p => p.Agreement)
                .Include(p => p.Order)
                .Include(p => p.ProductArticleNavigation)
                .ToListAsync();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };

            string responseData = JsonSerializer.Serialize(productparts, options);

            return Ok(responseData);
        }

        // GET: api/Productparts/5
        [HttpGet("productparts/{id}")]
        [ProducesResponseType(typeof(Productpart), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProductPart(int id)
        {
            var productPart = await _context.Productparts.FindAsync(id);
            if (productPart == null)
            {
                return NotFound();
            }
            return Ok(productPart);
        }

        // POST: api/ProductParts
        [HttpPost("productpartspost")]
        [ProducesResponseType(typeof(Productpart), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddProductPart([FromBody] Productpart productPart)
        {
            if (productPart == null)
            {
                return BadRequest("Объект табличной части для товара пуст.");
            }

            _context.Productparts.Add(productPart);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductPart), new { id = productPart.ProductPartId }, productPart);
        }
    }
}
