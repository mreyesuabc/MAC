using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace MAC.Controllers
{
    public class BaseController : Controller
    {
        protected string NombreCompleto => HttpContext.Session.GetString("NombreCompleto") ?? "Usuario";
        protected string Roles => HttpContext.Session.GetString("Roles") ?? "";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Pasar los datos a la vista
            ViewBag.NombreCompleto = NombreCompleto;
            ViewBag.Roles = Roles;

            base.OnActionExecuting(context);
        }

    }
}
