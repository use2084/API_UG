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
    public class ProductcategoriesController : ControllerBase
    {
        private readonly GidromashContext _context;

        public ProductcategoriesController(GidromashContext context)
        {
            _context = context;
        }

        // GET: api/Productcategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Productcategory>>> GetProductcategories()
        {
            return await _context.Productcategories.ToListAsync();
        }

        // GET: api/Productcategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Productcategory>> GetProductcategory(int id)
        {
            var productcategory = await _context.Productcategories.FindAsync(id);

            if (productcategory == null)
            {
                return NotFound();
            }

            return productcategory;
        }

        // PUT: api/Productcategories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductcategory(int id, Productcategory productcategory)
        {
            if (id != productcategory.ProductCategoryId)
            {
                return BadRequest();
            }

            _context.Entry(productcategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductcategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Productcategories
        [HttpPost]
        public async Task<ActionResult<Productcategory>> PostProductcategory(Productcategory productcategory)
        {
            _context.Productcategories.Add(productcategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductcategory", new { id = productcategory.ProductCategoryId }, productcategory);
        }

        // DELETE: api/Productcategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductcategory(int id)
        {
            var productcategory = await _context.Productcategories.FindAsync(id);
            if (productcategory == null)
            {
                return NotFound();
            }

            _context.Productcategories.Remove(productcategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductcategoryExists(int id)
        {
            return _context.Productcategories.Any(e => e.ProductCategoryId == id);
        }
    }
}