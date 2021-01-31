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
    [Owned]
    public class Direccion
    {
        [Required]
        [StringLength(100)]
        public string CallePrincipal { get; set; }

        [StringLength(100)]
        public string CalleSecundaria { get; set; }

        [StringLength(20)]
        public string Numeracion { get; set; }

        [StringLength(30)]
        public string Longitud { get; set; }

        [StringLength(30)]
        public string Latitud { get; set; }
    }
}
