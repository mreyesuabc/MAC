using MAC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;

namespace MAC.Controllers
{
    public class LoginController : Controller
    {
        private readonly MACDbContext _context;

        public LoginController(MACDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string usuarioapp, string password)
        {
            var usuario = _context.Usuarios
            .Where(u => u.usuarioapp != null && u.password != null)
            .FirstOrDefault(u => u.usuarioapp == usuarioapp && u.password == password);

            if (usuario == null)
            {

                ViewBag.ErrorLogin = "Usuario o contraseña incorrectos.";
                return View();

            }

            // Obtener roles del usuario
            var roles = (from ur in _context.UsuariosRol
                         join r in _context.Rol on ur.RolId equals r.Id
                         where ur.UsuarioId == usuario.id
                         select r).ToList();



            // Obtener procesos permitidos por los roles del usuario
            var procesos = (from rp in _context.RolProcesos
                            join p in _context.Procesos on rp.ProcesoId equals p.Id
                            where roles.Select(r => r.Id).Contains(rp.RolId)
                            && p.activo == 1
                            select new
                            {   p.Id,
                                p.descr,
                                p.controlador,
                                p.accion,
                                p.icono,
                                p.orden,
                                p.activo,
                                padreId = p.padreId ?? ""
                            }).Distinct().ToList();

            // Guardar en sesión
            HttpContext.Session.SetString("NombreCompleto", usuario.nombre + ' ' + usuario.appaterno + ' ' + usuario.apmaterno);
            HttpContext.Session.SetString("Roles", string.Join(", ", roles.Select(r => r.descr)));
            HttpContext.Session.SetString("UsuarioApp", usuario.usuarioapp);
            HttpContext.Session.SetInt32("Id", usuario.id);

            // Serializar procesos y guardar en sesión
            var procesosJson = JsonSerializer.Serialize(procesos);
            HttpContext.Session.SetString("ProcesosPermitidos", procesosJson);

            // Redirigir a la vista principal
            return RedirectToAction("Index", "Menu");
        }
    }
}
