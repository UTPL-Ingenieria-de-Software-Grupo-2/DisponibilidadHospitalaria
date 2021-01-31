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
    [Table("Instituciones")]
    [Index(nameof(Nombre), IsUnique = true)]
    public class Institucion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(TipoDeInstitucion))]
        public int TipoDeInstitucionId { get; set; }

        [Required]
        [ForeignKey(nameof(Ciudad))]
        public int CiudadId { get; set; }

        [Required]
        [StringLength(200)]
        public string Nombre { get; set; }

        [Required]
        public Direccion Direccion { get; set; }

        public TipoDeInstitucion TipoDeInstitucion { get; set; }

        public Ciudad Ciudad { get; set; }

        public List<Unidad> Unidades { get; set; }
    }
}
