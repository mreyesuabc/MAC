using MAC.Models;
using Microsoft.AspNetCore.Mvc;
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

        // Acción para editar un proceso
        public IActionResult Edit(int id)
        {
            var proceso = _context.Procesos.Find(id);
            if (proceso == null)
            {
                return NotFound();
            }

            return View(proceso);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Proceso proceso)
        {
            if (id != proceso.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Update(proceso);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(proceso);
        }
    }
    }