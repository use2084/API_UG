using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_UG.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace API_UG.Controllers
{
    public class OrdersController : ControllerBase
    {
        private readonly GidromashContext _context;

        public OrdersController(GidromashContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet("orders")]
        [ProducesResponseType(typeof(IEnumerable<Order>), 200)]
        public async Task<IActionResult> GetOrders()
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            var orders = await _context.Orders
                .Include(o => o.OrderCompanyNavigation)
                .Include(o => o.Serviceparts) 
                .Include(o => o.Productparts) 
                .ToListAsync();

            return Ok(JsonSerializer.Serialize(orders, options));
        }

        // GET: api/Orders/5
        [HttpGet("orders/{id}")]
        [ProducesResponseType(typeof(Order), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderCompanyNavigation)
                .FirstOrDefaultAsync(m => m.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // POST: api/Orders
        [HttpPost("orderspost")]
        [ProducesResponseType(typeof(Order), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddOrder([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest("Объект заказа пуст.");
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
        }
    }
}
