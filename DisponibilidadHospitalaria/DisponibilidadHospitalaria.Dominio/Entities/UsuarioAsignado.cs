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
    [Table("UsuariosAsignados")]
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Email), nameof(InstitucionId), IsUnique = true)]
    public class UsuarioAsignado
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(256)]
        public string Email { get; set; }

        [Required]
        public bool Activo { get; set; }

        [Required]
        public bool EsAdministrador { get; set; }

        [ForeignKey(nameof(Institucion))]
        public int? InstitucionId { get; set; }

        public Institucion Institucion { get; set; }
    }
}
