using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_UG.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace API_UG.Controllers
{
    public class AgreementsController : ControllerBase
    {
        private readonly GidromashContext _context;

        public AgreementsController(GidromashContext context)
        {
            _context = context;
        }

        // GET: api/Agreements
        [HttpGet("Agreements")]
        [ProducesResponseType(typeof(IEnumerable<Agreement>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAgreements()
        {
            var agreements = await _context.Agreements
    .Include(e => e.AgreementEmployeeNavigation)
    .Include(s => s.AgreementCompanyNavigation)
    .Include(a => a.Serviceparts) // Включить табличную часть для услуг
    .Include(a => a.Productparts) // Включить табличную часть для товаров
    .ToListAsync();


            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            return Ok(JsonSerializer.Serialize(agreements, options));
        }

        [HttpGet("Agreements/{id}")]
        [ProducesResponseType(typeof(Agreement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAgreement(int id)
        {
            var agreement = await _context.Agreements
                .Include(e => e.AgreementEmployeeNavigation)
                .Include(s => s.AgreementCompanyNavigation)
                .FirstOrDefaultAsync(a => a.AgreementId == id);

            if (agreement == null)
            {
                return NotFound();
            }

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            return Ok(JsonSerializer.Serialize(agreement, options));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Agreement), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAgreement([FromBody] Agreement agreement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Agreements.Add(agreement);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAgreement), new { id = agreement.AgreementId }, agreement);
        }
    }
}