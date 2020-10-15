using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Helpers
{
    public class FiltroPersonalizadoDeAccion : IActionFilter
    {
        private readonly ILogger<FiltroPersonalizadoDeAccion> logger;

        public FiltroPersonalizadoDeAccion(ILogger<FiltroPersonalizadoDeAccion> logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogError("OnActionExecuted");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogError("OnActionExecuting");
        }
    }
}
