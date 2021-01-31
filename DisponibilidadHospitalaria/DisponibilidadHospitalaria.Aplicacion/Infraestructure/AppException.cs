using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion
{
    public class AppException : Exception
    {
        public HttpStatusCode Code { get; }

        public object Errores { get; }


        public AppException(HttpStatusCode code, object errores = null)
        {
            Code = code;
            Errores = errores;
        }
    }
}
