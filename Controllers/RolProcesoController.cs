using MAC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MAC.Controllers
{
    public class RolProcesoController : Controller
    {
        private readonly MACDbContext _context;

        public RolProcesoController(MACDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var RolProcesos = _context.RolProcesos
                                   .OrderBy(p => p.RolId)
                                   .ToList();
            ViewBag.CurrentController = "RolProceso";
            ViewBag.CurrentAction = "Index";
            return View(RolProcesos);
        }
    }
}
