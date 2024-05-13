using Microsoft.EntityFrameworkCore;

namespace Emprendimiento_Api.Context
{
    public class ConexionSQLServer: DbContext
    {
        public ConexionSQLServer(DbContextOptions<ConexionSQLServer> options) : base(options)
        {

        }

        public DbSet<Models.Emprendimiento> Emprendimiento { get; set; }
    }
}
