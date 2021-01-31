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
    public class EditarCiudadModel : PageModelBase
    {

        [BindProperty]
        public CiudadCreateUpdate.RequestModel Input { get; set; }

        [ViewData]
        public List<ProvinciaDto> Provincias { get; set; }

        public async Task OnGetAsync(int id)
        {
            Input = Mapper.Map<CiudadCreateUpdate.RequestModel>(await Mediator.Send(new CiudadGet.RequestModel { Id = id }));
            Provincias = await Mediator.Send(new ProvinciasList.RequestModel());
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                await Mediator.Send(Input);
                return Redirect("./Index");
            }
            else
            {
                await OnGetAsync(Input.Id);
                return Page();
            }
        }
    }
}
