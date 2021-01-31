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
    [Table("Disponibilidades")]
    [Index(nameof(UnidadId), nameof(Fecha))]
    public class Disponibilidad
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Unidad))]
        public int UnidadId { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public int Ocupadas { get; set; }

        [Required]
        public int Disponibles { get; set; }

        public Unidad Unidad { get; set; }
    }
}
