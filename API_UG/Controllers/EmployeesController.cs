using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_UG.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace API_UG.Controllers
{
    public class EmployeesController : ControllerBase
    {
        private readonly GidromashContext _context;

        public EmployeesController(GidromashContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet("employees")]
        [ProducesResponseType(typeof(IEnumerable<Employee>), 200)]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _context.Employees
                .Include(e => e.EmployeePostNavigation)
                .Include(s => s.EmployeeStatusNavigation)
                .ToListAsync();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };

            string responseData = JsonSerializer.Serialize(employees, options);

            return Ok(responseData);
        }

        // GET: api/Employees/5
        [HttpGet("employees/{id}")]
        [ProducesResponseType(typeof(Employee), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // POST: api/Employees
        [HttpPost]
        [ProducesResponseType(typeof(Employee), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employee);
        }

        [HttpPost("AddEmployee")]
        public async Task<ActionResult<Employee>> AddEmployee(Employee employee)
        {
            if (employee == null)
            {
                return BadRequest("Объект сотрудника пуст.");
            }


            if (_context.Employees.Any(e => e.EmployeePhone == employee.EmployeePhone))
            {
                return Conflict("Сотрудник с таким номером телефона уже существует.");
            }

            if (_context.Employees.Any(e => e.EmployeePassport == employee.EmployeePassport))
            {
                return Conflict("Сотрудник с таким серийным номером паспорта уже существует.");
            }
            if (_context.Employees.Any(e => e.EmployeeLogin == employee.EmployeeLogin))
            {
                return Conflict("Сотрудник с таким логином уже существует.");
            }

            _context.Employees.Add(employee);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetEmployees), new { id = employee.EmployeeId }, employee);
        }

    }
}