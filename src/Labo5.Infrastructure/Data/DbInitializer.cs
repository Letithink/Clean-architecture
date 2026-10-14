using Labo5.Domain.Entities;

namespace Labo5.Infrastructure.Data;

public static class DbInitializer
{
    public static void Seed(Lab05Context context)
    {
        // Verificar si ya existen usuarios en la base de datos
        if (!context.Usuarios.Any())
        {
            // Crear usuario administrador por defecto si la tabla está vacía
            var adminUser = new Usuario
            {
                Usuario1 = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Rol = "Admin",
                FechaCreacion = DateTime.Now
            };

            context.Usuarios.Add(adminUser);
            context.SaveChanges();
        }
    }
}
