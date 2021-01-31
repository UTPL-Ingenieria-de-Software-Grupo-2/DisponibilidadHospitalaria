using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion;
using Aplicacion.Ciudades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DisponibilidadHospitalaria.Pages.Ciudades
{
    [Authorize(Roles = AppConstants.AdminsitradorRole)]
    public class IndexModel : PageModelBase
    {
        public List<CiudadDto> Ciudades { get; set; }

        public async Task OnGetAsync()
        {
            Ciudades = await Mediator.Send(new CiudadesList.RequestModel() { Filtro = "" });
        }
    }
}
