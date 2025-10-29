using MAC.Models;
using MAC.Models.ViewModels;
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
        [HttpPost]
        public IActionResult AgregarProceso(int rolId, int procesoId)
        {
            try
            {
                var nuevo = new RolProceso
                {
                    RolId = rolId,
                    ProcesoId = procesoId
                };

                _context.RolProcesos.Add(nuevo);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Proceso agregado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al agregar el proceso: " + ex.Message;
            }

            return RedirectToAction("ProcesosAsignados", new { rolId });
        }
        [HttpPost]
        public IActionResult EliminarProceso(int rolId, int procesoId)
        {
            try
            {
                var registro = _context.RolProcesos.FirstOrDefault(rp => rp.RolId == rolId && rp.ProcesoId == procesoId);
                if (registro != null)
                {
                    _context.RolProcesos.Remove(registro);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Proceso eliminado correctamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Registro no encontrado.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al eliminar el proceso: " + ex.Message;
            }

            return RedirectToAction("ProcesosAsignados", new { rolId });
        }
        public IActionResult IndexRolProceso()
        {

            ViewBag.NombreCompleto = HttpContext.Session.GetString("NombreCompleto") ?? "Usuario";
            ViewBag.Roles = HttpContext.Session.GetString("Roles") ?? "";

            var listaRoles = _context.Rol
                 .Select(g => new RolViewModel
                 {
                     RolId = g.Id,
                     RolDescr = g.descr
                 })
                .ToList();
            ViewBag.CurrentController = "RolProceso";
            ViewBag.CurrentAction = "IndexRolProceso"; // o el nombre de la acción que estás usando
            return View(listaRoles);
        }
        public IActionResult ProcesosAsignados(int rolId)
        {
            var procesosAsignados = (from rp in _context.RolProcesos
                                     join p in _context.Procesos on rp.ProcesoId equals p.Id
                                     where rp.RolId == rolId
                                     select new ProcesoAsignadoViewModel
                                     {
                                         ProcesoId = p.Id,
                                         NombreMenu = p.descr
                                     }).ToList();

            var procesosNoAsignados = (from p in _context.Procesos
                                       where !_context.RolProcesos
                                           .Any(rp => rp.ProcesoId == p.Id && rp.RolId == rolId)
                                       select new ProcesoNoAsignadoViewModel
                                       {
                                           ProcesoId = p.Id,
                                           NombreMenu = p.descr
                                       }).ToList();

            var model = new ProcesoRolViewModel
            {
                RolId = rolId,
                ProcesosAsignados = procesosAsignados,
                ProcesosNoAsignados = procesosNoAsignados
            };

            return PartialView("_VerProcesosRol", model);
        }

        //public IActionResult ProcesosAsignados(int rolId)
        //{
        //    // Procesos asignados al rol
        //    var procesosAsignados = (from rp in _context.RolProcesos
        //                             join p in _context.Procesos on rp.ProcesoId equals p.Id
        //                             where rp.RolId == rolId
        //                             select new ProcesoAsignadoViewModel
        //                             {
        //                                 ProcesoId = p.Id,
        //                                 NombreMenu = p.descr
        //                             }).ToList();

        //    // Procesos NO asignados al rol
        //    var procesosNoAsignados = (from p in _context.Procesos
        //                               where !_context.RolProcesos
        //                                   .Any(rp => rp.ProcesoId == p.Id && rp.RolId == rolId)
        //                               select new ProcesoNoAsignadoViewModel
        //                               {
        //                                   ProcesoId = p.Id,
        //                                   NombreMenu = p.descr
        //                               }).ToList();

        //    var model = new ProcesoRolViewModel
        //    {
        //        RolId = rolId,
        //        ProcesosAsignados = procesosAsignados,
        //        ProcesosNoAsignados = procesosNoAsignados
        //    };

        //    return PartialView("_VerProcesosRol", model);
        //}        
        public IActionResult AsignacionProcesos(int? rolId)
        {
            // Obtener lista de roles para el combo
            var roles = _context.Rol.ToList();
            ViewBag.Roles = new SelectList(roles, "Id", "descr"); // Ajusta el nombre del campo descriptivo

            // Consulta base sobre RolProceso
            var asignaciones = _context.RolProcesos
                .Include(rp => rp.ProcesoId)   // Relación para mostrar nombre del proceso
                .AsQueryable();

            // Filtrar si se selecciona un rol
            if (rolId.HasValue)
            {
                asignaciones = asignaciones.Where(rp => rp.RolId == rolId.Value);
            }

            return View(asignaciones.ToList());
        }
       
    }
}
