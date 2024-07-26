using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Ejercicio5Modulo3.Data;
using Ejercicio5Modulo3.Models;

namespace Ejercicio5Modulo3.Controllers { 

    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Método POST
        [HttpPost("seed")]
        public async Task<IActionResult> Seed()
        {
            // Verificar si ya hay datos en la tabla
            /* if (_context.Usuarios.Any())
            {
                return BadRequest("La tabla ya contiene datos.");
            }*/

            // Reiniciar el valor de identidad (OPCIONAL)
            // Se puede usar directamente (DBCC CHECKIDENT ('Users', RESEED, 0);) en SQL SERVER
            // await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Users', RESEED, 0)");

            var client = new HttpClient();
            var resultNumber = int.Parse(_configuration["SEED_RESULT_NUMBER"]);
            var response = await client.GetStringAsync($"https://randomuser.me/api/?results={resultNumber}");
            var users = JsonConvert.DeserializeObject<RandomUserResponse>(response).Results;

            foreach (var user in users)
            {
                // Verificar si el usuario ya existe en la base de datos
                if (!_context.Usuarios.Any(u =>
                    u.Nombre == user.Name.First &&
                    u.Apellido == user.Name.Last &&
                    u.Genero == user.Gender &&
                    u.Edad == user.Dob.Age &&
                    u.Email == user.Email &&
                    u.NombreUsuario == user.Login.Username &&
                    u.Ciudad == user.Location.City &&
                    u.Estado == user.Location.State &&
                    u.Pais == user.Location.Country))
                {
                    _context.Usuarios.Add(new User
                    {
                        Nombre = user.Name.First,
                        Apellido = user.Name.Last,
                        Genero = user.Gender,
                        Edad = user.Dob.Age,
                        Email = user.Email,
                        NombreUsuario = user.Login.Username,
                        Password = user.Login.Password,
                        Ciudad = user.Location.City,
                        Estado = user.Location.State,
                        Pais = user.Location.Country
                    });
                }
            }

            await _context.SaveChangesAsync();
            return Ok("Database seeded successfully");
        }

        // Método GET
        // Permitir que se permitan valores nulos usando ?
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? genero = null, [FromQuery] string? pais = null)
        {
            var query = _context.Usuarios.AsQueryable();

            if (!string.IsNullOrEmpty(genero))
            {
                query = query.Where(u => u.Genero == genero);
            }

            if (!string.IsNullOrEmpty(pais))
            {
                query = query.Where(u => u.Pais == pais);
            }

            var usuarios = await query.ToListAsync();
            return Ok(usuarios);
        }
    }

    public class RandomUserResponse
    {
        public List<RandomUser> Results { get; set; }
    }

    public class RandomUser
    {
        public Name Name { get; set; }
        public string Gender { get; set; }
        public Dob Dob { get; set; }
        public string Email { get; set; }
        public Login Login { get; set; }
        public Location Location { get; set; }
    }

    public class Name
    {
        public string First { get; set; }
        public string Last { get; set; }
    }

    public class Dob
    {
        public int Age { get; set; }
    }

    public class Login
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class Location
    {
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
