using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia
{
    public class ServiceRegister
    {
        public ServiceRegister(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<InitializeOptions>(configuration.GetSection("DatabaseInitializeOptions"));

            services.AddTransient<IDatabaseMigrate, DatabaseMigrate>();
            services.AddTransient<IDatabaseInitialize, DatabaseInitialize>();
        }
    }
}
