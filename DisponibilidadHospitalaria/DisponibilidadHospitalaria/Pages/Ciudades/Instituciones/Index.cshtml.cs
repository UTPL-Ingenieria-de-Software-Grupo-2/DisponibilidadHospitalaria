using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion;
using Aplicacion.Ciudades;
using Aplicacion.Instituciones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DisponibilidadHospitalaria.Pages.Ciudades.Instituciones
{
    [Authorize(Roles = AppConstants.AdminsitradorRole)]
    public class Index : PageModelBase
    {
        public CiudadDto Ciudad { get; set; }

        public async Task OnGetAsync(int ciudadId)
        {
            Ciudad = await Mediator.Send(new CiudadGet.RequestModel() { Id = ciudadId });
        }

    }
}
