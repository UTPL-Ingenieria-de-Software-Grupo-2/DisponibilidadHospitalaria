using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia
{
    public interface IDatabaseMigrate
    {
        void Migrate();
    }

    public class DatabaseMigrate : IDatabaseMigrate
    {
        private readonly ApplicationDbContext _context;

        public DatabaseMigrate(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Migrate()
        {
            _context.Database.MigrateAsync().Wait();
        }
    }
}
