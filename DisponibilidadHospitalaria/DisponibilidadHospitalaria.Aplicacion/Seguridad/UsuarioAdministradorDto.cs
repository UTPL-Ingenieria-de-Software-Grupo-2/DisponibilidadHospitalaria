﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class UsuarioAdministradorDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public bool Activo { get; set; }
    }
}
