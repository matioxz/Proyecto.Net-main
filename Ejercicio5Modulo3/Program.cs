using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Ejercicio5Modulo3.Data;

namespace Ejercicio5Modulo3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configurar el DbContext para usar SQL Server
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Agregar servicios para controladores
            builder.Services.AddControllers();

            // Configurar Swagger para la documentación de la API
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            var app = builder.Build();

            // Configurar el pipeline de solicitudes HTTP
            if (app.Environment.IsDevelopment())
            {
                // Usar Swagger solo en desarrollo
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
            }

            // Redirección HTTPS (comentado)
            // app.UseHttpsRedirection();

            // Usar autorización
            app.UseAuthorization();

            // Mapear controladores
            app.MapControllers();

            // Ejecutar la aplicación
            app.Run();
        }
    }
}
