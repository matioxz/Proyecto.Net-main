using Ejercicio5Modulo3.Models;
using Microsoft.EntityFrameworkCore;

namespace Ejercicio5Modulo3.Data
{
    // Clase que representa el contexto de la base de datos
    public class AppDbContext : DbContext
    {
        // Definición del conjunto de datos para los usuarios
        public DbSet<User> Usuarios { get; set; }

        // Constructor que recibe opciones de configuración
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de la entidad User para mapear a la tabla "Usuario"
            modelBuilder.Entity<User>().ToTable("Usuario");
            modelBuilder.Entity<User>(entity =>
            {
                // Definición de la clave primaria
                entity.HasKey(e => e.Id);

                // Configuración de propiedades con restricciones
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Apellido).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Edad).IsRequired();
                entity.Property(e => e.Genero).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.NombreUsuario).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Ciudad).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Estado).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Pais).IsRequired().HasMaxLength(100);
            });
        }
    }
}
