using Aplicacion.Instituciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Disponibilidades
{
    public class DisponibilidadDto
    {
        public int Id { get; set; }
        public int UnidadId { get; set; }
        public DateTime Fecha { get; set; }
        public int Ocupadas { get; set; }
        public int Disponibles { get; set; }
    }
}
