using API_UG.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<GidromashContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")));
});

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.Cookie.Name = "MyCsrfCookie";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(options =>
{
    options.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

app.UseAuthorization();

app.MapControllers();

app.Run();

var jwtSecret = builder.Configuration["Jwt:Secret"];
var secretKey = Encoding.ASCII.GetBytes(jwtSecret);
var signingKey = new SymmetricSecurityKey(secretKey);

// Метод для создания файла куки с данными компании-клиента
void CreateCompanyCookieFile(HttpContext httpContext, Company company)
{
    string data = $"{company.CompanyName}\n{company.CompanyDelegate}\n{company.CompanyInn}\n{company.CompanyAddress}";
    string path = Path.Combine(Directory.GetCurrentDirectory(), "company_cookie.txt");
    File.WriteAllText(path, data);
    httpContext.Response.Cookies.Append("CompanyCookie", "company_cookie.txt");
}

// Метод для удаления файла куки с данными компании-клиента
void DeleteCompanyCookieFile(HttpContext httpContext)
{
    string path = Path.Combine(Directory.GetCurrentDirectory(), "company_cookie.txt");
    if (File.Exists(path))
    {
        File.Delete(path);
    }
}
