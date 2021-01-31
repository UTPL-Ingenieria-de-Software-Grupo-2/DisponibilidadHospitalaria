using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entities
{
    [Table("Ciudades")]
    [Index(nameof(ProvinciaId), nameof(Nombre), IsUnique = true)]
    public class Ciudad
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Provincia))]
        public int ProvinciaId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        public Provincia Provincia { get; set; }

        public List<Institucion> Instituciones { get; set; }
    }
}
