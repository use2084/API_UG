using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_UG.Models;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.Web;

namespace API_UG.Controllers
{
    public class CompaniesController : ControllerBase
    {
        private readonly GidromashContext _context;
        private readonly IConfiguration _configuration;

        public CompaniesController(GidromashContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Companies
        [HttpGet("companies")]
        [ProducesResponseType(typeof(IEnumerable<Company>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _context.Companies.ToListAsync();
            return Ok(companies);
        }

        // GET: api/Companies/5
        [HttpGet("company/{id}")]
        [ProducesResponseType(typeof(Company), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCompany(int id)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(m => m.CompanyId == id);

            if (company == null)
            {
                return NotFound();
            }

            return Ok(company);
        }

        [HttpPost("loginCompany")]
        public async Task<IActionResult> LoginClient([FromBody] Company clientLoginDTO)
        {
            if (clientLoginDTO == null)
            {
                return BadRequest("Не удалось получить данные для аутентификации клиента.");
            }

            var client = await _context.Companies.SingleOrDefaultAsync(c => c.CompanyLogin == clientLoginDTO.CompanyLogin);

            if (client == null || !VerifyPassword(clientLoginDTO.CompanyPassword, client.CompanyPassword))
            {
                return Unauthorized("Неверные учетные данные клиента.");
            }

            return Ok(new { CompanyId = client.CompanyId, CompanyEmailAddress = client.CompanyEmailAddress, CompanyName = client.CompanyName });
        }

        private bool VerifyPassword(string inputPassword, string storedPasswordHash)
        {
            return inputPassword == storedPasswordHash;
        }

        [HttpPost("sendEmail")]
        public async Task<IActionResult> SendEmailToClient([FromBody] EmailDTO emailDTO)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("rozalina4rslanova@yandex.ru", "ООО Уфагидромаш");
                mail.To.Add(emailDTO.ClientEmail);
                mail.Subject = "Заказ";
                mail.Body = ConstructEmailBody(emailDTO);
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 587);
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential("rozalina4rslanova@yandex.ru", "neafqhylewnmoiny");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtp.Send(mail);

                return Ok("Письмо успешно отправлено на почту клиента.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка при отправке письма на почту клиента: {ex.Message}");
            }
        }

        [HttpPost("ordersPostM")]
        public async Task<IActionResult> PostOrder([FromBody] OrderDTO orderDTO)
        {
            if (orderDTO == null || orderDTO.OrderCompany == null)
            {
                return BadRequest("Не удалось получить данные заказа.");
            }

            var order = new Order
            {
                OrderCompany = orderDTO.OrderCompany,
                OrderDate = DateOnly.FromDateTime(DateTime.Today),
                OrderCost = orderDTO.OrderCost,
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in orderDTO.CartItems)
            {
                var productPart = new Productpart
                {
                    ProductArticle = item.ProductId,
                    QuantityProduct = item.Quantity,
                    OrderId = order.OrderId
                };
                _context.Productparts.Add(productPart);
            }

            foreach (var item in orderDTO.ServiceItems)
            {
                var servicePart = new Servicepart
                {
                    ServiceId = item.ServiceId,
                    ServiceQuantity = item.Quantity,
                    OrderId = order.OrderId
                };
                _context.Serviceparts.Add(servicePart);
            }

            await _context.SaveChangesAsync();

            var emailDTO = new EmailDTO
            {
                OrderId = order.OrderId,
                ClientEmail = orderDTO.ClientEmail,
                CartItems = orderDTO.CartItems,
                ServiceItems = orderDTO.ServiceItems,
                OrderCost = orderDTO.OrderCost,
                OrderDate = DateOnly.FromDateTime(DateTime.Today),
                CompanyName = orderDTO.CompanyName
            };
            await SendEmailToClient(emailDTO);
            return Ok(new { orderId = order.OrderId });
        }
        private string ConstructEmailBody(EmailDTO emailDTO)
        {
            string formattedDate = emailDTO.OrderDate.ToString("dd.MM.yyyy");

            string body = $"<p>Здравствуйте, {HttpUtility.HtmlEncode(emailDTO.CompanyName)}!</p>";
            body += "<p>Ваш заказ успешно оформлен.</p>";
            body += $"<p>Номер заказа: {emailDTO.OrderId}</p>";
            body += $"<p>Дата заказа: {formattedDate}</p>";

            if (emailDTO.CartItems.Any())
            {
                body += "<h3>Товары:</h3><ul>";
                foreach (var item in emailDTO.CartItems)
                {
                    body += $"<li><b>{HttpUtility.UrlDecode(item.ProductName)}</b>, {item.Quantity} шт., {item.ProductPrice} руб.</li>";
                }
                body += "</ul>";
            }

            if (emailDTO.ServiceItems.Any())
            {
                body += "<h3>Услуги:</h3><ul>";
                foreach (var item in emailDTO.ServiceItems)
                {
                    body += $"<li><b>{HttpUtility.UrlDecode(item.ServiceName)}</b>, {item.Quantity} шт., {item.ServicePrice} руб.</li>";
                }
                body += "</ul>";
            }

            body += $"<p>Общая стоимость заказа: {emailDTO.OrderCost} руб.</p>";
            body += "<p>Спасибо за ваш заказ!</p>";
            body += "<p>С уважением, ООО Уфагидромаш.</p>";

            return body;
        }
    }

    public class EmailDTO
    {
        public int OrderId { get; set; }
        public string ClientEmail { get; set; }
        public List<CartItemDTO> CartItems { get; set; }
        public List<ServiceItemDTO> ServiceItems { get; set; }
        public decimal OrderCost { get; set; }
        public DateOnly OrderDate { get; set; }
        public string CompanyName { get; set; } 
    }

    public class OrderDTO
    {
        public int OrderCompany { get; set; }
        public DateOnly OrderDate { get; set; }
        public int OrderCost { get; set; }
        public List<CartItemDTO> CartItems { get; set; }
        public List<ServiceItemDTO> ServiceItems { get; set; }
        public string ClientEmail { get; set; }
        public string CompanyName { get; set; }
    }

    public class CartItemDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal ProductPrice { get; set; }
    }

    public class ServiceItemDTO
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int Quantity { get; set; }
        public decimal ServicePrice { get; set; }
    }
}
