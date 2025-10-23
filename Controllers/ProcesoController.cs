using MAC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace TuProyecto.Controllers
{
    public class ProcesoController : Controller
    {
        private readonly MACDbContext _context;

        public ProcesoController(MACDbContext context)
        {
            _context = context;
        }

        // Acción para mostrar la lista de procesos
        public IActionResult Index()
        {
            var procesos = _context.Procesos
                                   .OrderBy(p => p.orden)
                                   .ToList();
            ViewBag.CurrentController = "Proceso";
            ViewBag.CurrentAction = "Index";
            return View(procesos);
        }
        public IActionResult EditarProcesoParcial(int id)
        {
            var proceso = _context.Procesos.FirstOrDefault(u => u.Id == id);
            if (proceso == null)
            {
                return NotFound();
            }            
            return PartialView("_EditarProceso", proceso);
        }
        [HttpPost]
        public IActionResult GuardarCambiosProceso(Proceso proceso)
        {
            var procesoDb = _context.Procesos.FirstOrDefault(u => u.Id == proceso.Id);
            if (procesoDb == null)
            {
                return NotFound();
            }

            procesoDb.descr = proceso.descr;
            procesoDb.controlador = proceso.controlador;
            procesoDb.accion = proceso.accion;
            procesoDb.icono = proceso.icono;
            procesoDb.orden = proceso.orden;
            procesoDb.padreId = proceso.padreId;
            try
            {
                _context.SaveChanges();
                TempData["Mensaje"] = "Se actualizó información correctamente.";

            }
            catch (Exception ex)
            {
                TempData["Errores"] = "No fue posible actualizar la información del proceso." + ex;
            }
            return Ok();
        }
        //// Acción para editar un proceso
        //public IActionResult Edit(int id)
        //{
        //    var proceso = _context.Procesos.Find(id);
        //    if (proceso == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(proceso);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(int id, Proceso proceso)
        //{
        //    if (id != proceso.Id)
        //    {
        //        return BadRequest();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        _context.Update(proceso);
        //        _context.SaveChanges();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    return View(proceso);
        //}
        [HttpPost]
        public IActionResult ActualizarActivo(int id, int activo)
        {
            var proceso = _context.Procesos.FirstOrDefault(p => p.Id == id);
            if (proceso == null)
            {
                return NotFound();
            }

            proceso.activo = activo;
            _context.SaveChanges();

            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> AddProceso(Proceso proceso)
        {
            if (ModelState.IsValid)
            {

                _context.Procesos.Add(proceso);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Proceso registrado correctamente.";
                ViewBag.CurrentController = "Proceso";
                ViewBag.CurrentAction = "Index";
                return RedirectToAction("Index");
            }
            var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            TempData["Errores"] = errores;
            ViewBag.CurrentController = "Proceso";
            ViewBag.CurrentAction = "Index";
            return RedirectToAction("Index");
        }
    }
    }