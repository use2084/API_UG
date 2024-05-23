using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using API_UG.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace API_UG.Controllers
{
    public class ProductsController : ControllerBase
    {
        private readonly GidromashContext _context;

        public ProductsController(GidromashContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet("products")]
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products
                .Include(p => p.ProductCategoryNavigation)
                .ToListAsync();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };

            string responseData = JsonSerializer.Serialize(products, options);

            return Ok(responseData);
        }

        // GET: api/Products/5
        [HttpGet("products/{id}")]
        [ProducesResponseType(typeof(Product), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST: api/Products
        [HttpPost("products")]
        [ProducesResponseType(typeof(Product), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductArticle }, product);
        }

        // POST: api/AddProduct
        [HttpPost("AddProduct")]
        public async Task<ActionResult<Product>> AddProduct(Product product)
        {
            if (product == null)
            {
                return BadRequest("Объект товара пуст.");
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducts), new { id = product.ProductArticle }, product);
        }
    }
}
