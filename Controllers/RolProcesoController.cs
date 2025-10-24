using MAC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            
            var rolProcesos = _context.RolProcesos
                .Include(rp => rp.rolInfo)
                .Include(rp => rp.procesoInfo)
                .ToList();
            ViewBag.CurrentController = "RolProceso";
            ViewBag.CurrentAction = "Index";
            return View(rolProcesos); // Aquí estás pasando una lista de RolProceso
        }
        public IActionResult AsignacionProcesos(int? rolId)
        {
            // Obtener lista de roles para el combo
            var roles = _context.Rol.ToList();
            ViewBag.Roles = new SelectList(roles, "Id", "descr"); // Ajusta el nombre del campo descriptivo

            // Consulta base sobre RolProceso
            var asignaciones = _context.RolProcesos
                .Include(rp => rp.RolId)       // Relación para mostrar nombre del rol
                .Include(rp => rp.ProcesoId)   // Relación para mostrar nombre del proceso
                .AsQueryable();

            // Filtrar si se selecciona un rol
            if (rolId.HasValue)
            {
                asignaciones = asignaciones.Where(rp => rp.RolId == rolId.Value);
            }

            return View(asignaciones.ToList());
        }
        public IActionResult VerProcesosPorRol(int rolId)
        {
            // Obtener los procesos asignados al rol
            var procesos = _context.RolProcesos
                .Include(rp => rp.procesoInfo) // Incluye la información del proceso
                .Where(rp => rp.RolId == rolId)
                .Select(rp => rp.ProcesoId) // Selecciona solo los procesos
                .ToList();

            // Retorna la vista parcial con la lista de procesos
            return PartialView("_ProcesosPorRol", procesos);
        }
    }
}
