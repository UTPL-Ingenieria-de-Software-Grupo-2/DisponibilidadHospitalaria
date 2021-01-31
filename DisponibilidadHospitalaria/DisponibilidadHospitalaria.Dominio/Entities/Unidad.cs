using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entities
{
    [Table("InstitucionesUnidades")]
    public class Unidad
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Institucion))]
        public int InstitucionId { get; set; }

        [Required]
        [ForeignKey(nameof(TipoDeUnidad))]
        public int TipoDeUnidadId { get; set; }

        [StringLength(100)]
        public string Denominacion { get; set; }

        [Required]
        public int Capacidad { get; set; }

        public TipoDeUnidad TipoDeUnidad { get; set; }

        public Institucion Institucion { get; set; }

        public List<Disponibilidad> Disponibilidades { get; set; }
    }
}
