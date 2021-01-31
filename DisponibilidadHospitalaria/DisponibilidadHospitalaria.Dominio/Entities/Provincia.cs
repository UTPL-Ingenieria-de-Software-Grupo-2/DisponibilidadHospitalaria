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
    [Table("Provincias")]
    [Index(nameof(Codigo), IsUnique = true)]
    [Index(nameof(Nombre), IsUnique = true)]
    public class Provincia
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(2)]
        public string Codigo { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
    }
}
