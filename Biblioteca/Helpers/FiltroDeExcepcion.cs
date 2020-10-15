using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Helpers
{
    public class FiltroDeExcepcion : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var mensaje = context.Exception.Message;
        }
    }
}
