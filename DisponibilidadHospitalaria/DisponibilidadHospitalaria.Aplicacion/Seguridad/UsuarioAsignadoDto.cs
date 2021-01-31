using Aplicacion.Instituciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class UsuarioAsignadoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public bool Activo { get; set; }
        public int InstitucionId { get; set; }
        public string Institucion_Nombre { get; set; }
        public string Institucion_Ciudad { get; set; }
        public string Institucion_Provincia { get; set; }
        public InstitucionDto Institucion { get; set; }
    }
}
