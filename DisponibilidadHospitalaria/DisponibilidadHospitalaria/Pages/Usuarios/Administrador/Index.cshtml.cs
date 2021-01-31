using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion;
using Aplicacion.Seguridad;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DisponibilidadHospitalaria.Pages.Usuarios.Administrador
{
    [Authorize(Roles=AppConstants.AdminsitradorRole)]
    public class IndexModel : PageModelBase
    {
        public List<UsuarioAsignadoDto> Usuarios { get; set; }

        public async Task OnGetAsync()
        {
            Usuarios = await Mediator.Send(new UsuariosAsignadosList.RequestModel() { Filtro = "" });
        }
    }
}
