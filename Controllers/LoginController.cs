using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MAC.Models;
using System.Linq;

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
            //            var usuario = _context.Usuarios.FirstOrDefault(u => u.usuarioapp == usuarioapp && u.password == password);
            var usuario = _context.Usuarios
            .Where(u => u.usuarioapp != null && u.password != null)
            .FirstOrDefault(u => u.usuarioapp == usuarioapp && u.password == password);

            if (usuario == null)
            {

                ModelState.AddModelError("", "Usuario o contraseña incorrectos");
                return View();

            }

            // Obtener roles del usuario
            var roles = (from ur in _context.UsuariosRol
                         join r in _context.Rol on ur.RolId equals r.Id
                         where ur.UsuarioId == usuario.id
                         select r.descr).ToList();



            // Guardar en sesión
            HttpContext.Session.SetString("NombreCompleto", usuario.nombre+' '+usuario.appaterno+' '+usuario.apmaterno);
            HttpContext.Session.SetString("Roles", string.Join(", ", roles));
            HttpContext.Session.SetString("UsuarioApp", usuario.usuarioapp);
            HttpContext.Session.SetInt32("Id", usuario.id);


            // Redirigir a la vista principal
            return RedirectToAction("Index", "Menu");
        }
    }
}
