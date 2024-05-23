using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API_UG.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API_UG.Controllers
{
    public class AuthEmployeesController : ControllerBase
    {
        private readonly GidromashContext _context;

        public AuthEmployeesController(GidromashContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] Models.Employee employeeLoginDTO)
        {
            if (employeeLoginDTO == null || string.IsNullOrEmpty(employeeLoginDTO.EmployeeLogin) || string.IsNullOrEmpty(employeeLoginDTO.EmployeePassword))
            {
                return BadRequest(new {message = "Введите логин и пароль."});
            }

            var  employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeLogin == employeeLoginDTO.EmployeeLogin && e.EmployeePassword == employeeLoginDTO.EmployeePassword);
            if(employee == null)
            {
                return Unauthorized();
            }
            else if (employee.EmployeeStatus == 2)
            {
                return Unauthorized(new { message = "У вас нет доступа к системе." });
            }
            else if (employee.EmployeePost == 1)
            {
                return Ok(new { id = $"{employee.EmployeeId}", fullname = $"{employee.EmployeeFirstName} {employee.EmployeeName} {employee.EmployeePatronymic}", post = $"Администратор" });
            }

            else if (employee.EmployeePost == 2)
            {
                return Ok(new { id = $"{employee.EmployeeId}", fullname = $"{employee.EmployeeFirstName} {employee.EmployeeName} {employee.EmployeePatronymic}", post = $"Оператор" });
            }
            else
            {
                return Ok(new { Role = "Не найдена роль" });
            }
        }
    }
}

