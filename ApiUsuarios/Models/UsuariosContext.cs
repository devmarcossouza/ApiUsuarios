using Microsoft.EntityFrameworkCore;

namespace ApiUsuarios.Models
{
    public class UsuariosContext : DbContext
    {
        public UsuariosContext(DbContextOptions<UsuariosContext> options)
            : base(options)
        {
        }

        public DbSet<Usuarios> Usuarios { get; set; }
        
    }
}
