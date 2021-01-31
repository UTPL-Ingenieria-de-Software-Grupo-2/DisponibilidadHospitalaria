using Dominio.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistencia
{
    public interface IDatabaseInitialize
    {
        void Initialize();
    }

    public class DatabaseInitialize : IDatabaseInitialize
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DatabaseInitialize> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly InitializeOptions _initializeOptions;

        public DatabaseInitialize(
            ApplicationDbContext context,
            ILogger<DatabaseInitialize> logger,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<InitializeOptions> options)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _initializeOptions = options.Value;
        }

        public void Initialize()
        {
            Task.Run(async () =>
            {
                await InitializeRoles();
                await InitializeUsers();
                await LoadInitialData();
            }).Wait();
        }

        private async Task InitializeRoles()
        {
            foreach (var x in _initializeOptions.RoleList.Split(',').ToList())
                if (!await _roleManager.RoleExistsAsync(x))
                    await _roleManager.CreateAsync(new IdentityRole(x));

            foreach (var r in _roleManager.Roles.Where(x => !_initializeOptions.RoleList.Contains(x.Name)).ToList())
                await _roleManager.DeleteAsync(r);
        }

        private async Task InitializeUsers()
        {
            if (!await _userManager.Users.AnyAsync())
            {
                var user = new ApplicationUser
                {
                    UserName = _initializeOptions.MasterUserName,
                    Email = _initializeOptions.MasterUserName,
                    Nombre = _initializeOptions.MasterNombre,
                    EmailConfirmed = true,
                    Activo = true
                };

                await _userManager.CreateAsync(user, _initializeOptions.MasterPassword);
                var newUser = await _userManager.FindByNameAsync(_initializeOptions.MasterUserName);
                await _userManager.AddToRoleAsync(newUser, _initializeOptions.MasterRole);

                _logger.LogInformation($"MasterUser [{_initializeOptions.MasterUserName}] creado");
            }
        }

        private async Task LoadInitialData()
        {
            await cargarData<Provincia>("Provincias.json");
            await cargarData<TipoDeInstitucion>("TiposDeInstitucion.json");
            await cargarData<TipoDeUnidad>("TiposDeUnidad.json");

            async Task cargarData<T>(string nombreDeRecurso) where T : class
            {
                if (!await _context.Set<T>().AnyAsync())
                {
                    var name = Assembly.GetExecutingAssembly().GetManifestResourceNames().First(x => x.EndsWith(nombreDeRecurso));
                    var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);

                    foreach (T x in await JsonSerializer.DeserializeAsync<List<T>>(stream))
                    {
                        await _context.AddAsync(x);
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }

    }
}
