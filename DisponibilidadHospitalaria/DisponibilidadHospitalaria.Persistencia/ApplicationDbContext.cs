using Dominio.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<Ciudad> Ciudades { get; set; }

        public DbSet<TipoDeInstitucion> TiposDeInstitucion { get; set; }
        public DbSet<TipoDeUnidad> TiposDeUnidad { get; set; }

        public DbSet<Institucion> Instituciones { get; set; }
        public DbSet<Unidad> Unidades { get; set; }
        public DbSet<Disponibilidad> Disponibilidades { get; set; }

        public DbSet<UsuarioAsignado> UsuariosAsignados { get; set; }
    }
}
