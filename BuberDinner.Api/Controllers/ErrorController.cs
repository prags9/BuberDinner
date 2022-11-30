using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuberDinner.Application.Common.Errors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers
{
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Error(){
            Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
            var (statuscode, message) = exception switch
            {
                IServiceException serviceEsception => ((int)serviceEsception.StatusCode, serviceEsception.ErrorMessage),
                _ => (StatusCodes.Status500InternalServerError, "An exception occurred")
            };
            return Problem(statusCode: statuscode, title: message);
        }
    }
}