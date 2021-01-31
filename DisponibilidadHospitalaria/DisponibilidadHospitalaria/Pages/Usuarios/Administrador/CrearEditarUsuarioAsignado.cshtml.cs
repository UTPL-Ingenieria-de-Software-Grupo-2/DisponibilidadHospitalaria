using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion;
using Aplicacion.Ciudades;
using Aplicacion.Instituciones;
using Aplicacion.Seguridad;
using Aplicacion.Seguridad.Users;
using DisponibilidadHospitalaria.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DisponibilidadHospitalaria.Pages.Usuarios.Administrador
{
    [Authorize(Roles = AppConstants.AdminsitradorRole)]
    public class CrearEditarUsuarioAsignadoModel : PageModelBase
    {

        [BindProperty]
        public UsuarioAsignadoCreateUpdate.RequestModel Input { get; set; }

        [ViewData]
        public List<ElgirInstitucionVM.Provincia> Provincias => ViewModel.Provincias;

        [ViewData]
        public List<ElgirInstitucionVM.Ciudad> Ciudades => ViewModel.Ciudades;

        [ViewData]
        public List<ElgirInstitucionVM.Institucion> Instituciones => ViewModel.Instituciones;

        private ElgirInstitucionVM ViewModel { get; set; } = null;

        public async Task OnGetAsync(int id = -1)
        {
            ViewModel = new ElgirInstitucionVM(await Mediator.Send(new CiudadesList.RequestModel()));

            if (id == -1)
                Input = new UsuarioAsignadoCreateUpdate.RequestModel() { Id = -1, Activo = true, };
            else
            {
                Input ??= Mapper.Map<UsuarioAsignadoCreateUpdate.RequestModel>(await Mediator.Send(new UsuarioAsignadoGet.RequestModel { Id = id }));
                ViewModel.SetInstitucionId(Input.InstitucionId);
            }

            Input.ProvinciaId = ViewModel.ProvinciaId;
            Input.CiudadId = ViewModel.CiudadId;
        }

        public async Task<IActionResult> OnGetCiudadesAsync(int id)
        {
            var vm = new ElgirInstitucionVM(await Mediator.Send(new CiudadesList.RequestModel())) { ProvinciaId = id };
            return new JsonResult(vm.Ciudades.Select(x => new { x.Id, x.Nombre }).OrderBy(x => x.Nombre).ToList());
        }

        public async Task<IActionResult> OnGetInstitucionesAsync(int id)
        {
            var vm = new ElgirInstitucionVM(await Mediator.Send(new CiudadesList.RequestModel())) { CiudadId = id };
            return new JsonResult(vm.Instituciones.Select(x => new { x.Id, x.Nombre }).OrderBy(x => x.Nombre).ToList());
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                await Mediator.Send(Input);
                await Mediator.Send(new UserVerificarRoles.RequestModel() { UserName = Input.Email });
                return Redirect("./Index");
            }
            else
            {
                await OnGetAsync(Input.Id);
                ViewModel = new ElgirInstitucionVM(await Mediator.Send(new CiudadesList.RequestModel()));
                ViewModel.SetInstitucionId(Input.InstitucionId);
                return Page();
            }
        }
    }
}
