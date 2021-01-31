using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entities
{
    [Table("TiposDeUnidad")]
    public class TipoDeUnidad
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Denominacion { get; set; }
    }
}
