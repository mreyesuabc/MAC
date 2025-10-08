using AspNetCoreGeneratedDocument;
using MAC.Models;
using MAC.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection.Metadata;
using System.Xml.Linq;

public class UsuarioController : Controller
{
    private readonly MACDbContext _context;

    public UsuarioController(MACDbContext context)
    {
        _context = context;
    }

    public IActionResult MiPerfil()
    {
        var usuarioApp = HttpContext.Session.GetString("UsuarioApp");

        if (string.IsNullOrEmpty(usuarioApp))
        {
            return RedirectToAction("Index", "Login");
        }

       var usuario = _context.Usuarios
            .Include(u => u.mpoInfo)
            .FirstOrDefault(u => u.usuarioapp == usuarioApp);
        if (usuario == null)
        {
            return RedirectToAction("Index", "Login");
        }

        ViewBag.CurrentController = "Usuario";
        ViewBag.CurrentAction = "MiPerfil";

        return View(usuario); // Pasa el modelo a la vista
    }

    // Acción para mostrar la lista de usuarios del sistema
    public IActionResult Usuarios()
    {
        var listaUsuarios = _context.Usuarios
            .Include(u => u.mpoInfo)
            .Include(u => u.rolInfo)
            .ToList();

        var viewModel = new UsuarioViewModel
        {
            Municipios = _context.Municipios
                .Select(m => new SelectListItem { Value = m.mpo.ToString(), Text = m.descr })
                .ToList(),
            Roles = _context.Rol
                .Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.descr })
                .ToList()
        };
        ViewBag.CurrentController = "Usuario";
        ViewBag.CurrentAction = "Usuarios";
        ViewBag.ViewModelUsuario = viewModel;

        return View(listaUsuarios);
    }
    public IActionResult EditarUsuarioParcial(int id)
    {
        var usuario = _context.Usuarios.FirstOrDefault(u => u.id == id);
        if (usuario == null)
        {
            return NotFound();
        }
        return PartialView("_EditarUsuario", usuario);
    }


[HttpPost]
public IActionResult GuardarCambiosUsuario(Usuario usuario)
    {
        var usuarioDb = _context.Usuarios.FirstOrDefault(u => u.id == usuario.id);
        if (usuarioDb == null)
        {
            return NotFound();
        }

        usuarioDb.correo = usuario.correo;
        usuarioDb.rfc = usuario.rfc;
        usuarioDb.homoclave = usuario.homoclave;
        usuarioDb.municipio = usuario.municipio;

        _context.SaveChanges();

        return Ok();
    }


    [HttpPost]
    public async Task<IActionResult> CambiarPassword([FromBody] CambioPswVm model)
    {
        var usuarioId = HttpContext.Session.GetInt32("Id");

        var usuario = await _context.Usuarios.FindAsync(usuarioId);

        if (usuario == null)
            return Json(new { mensaje = "Usuario no encontrado." });

        if (usuario.password != model.Actual)
            return Json(new { mensaje = "La contraseña actual es incorrecta." });

        usuario.password = model.Nueva; 
        _context.Update(usuario);
        await _context.SaveChangesAsync();

        return Json(new { mensaje = "Contraseña actualizada correctamente." });
    }
    [HttpPost]
    public async Task<IActionResult> AddUsr(UsuarioViewModel modal)
    {
        ModelState.Remove("Municipios");
        ModelState.Remove("Roles");
        if (ModelState.IsValid)
        {
            var usr = new Usuario
            {
                usuarioapp = modal.UsuarioApp,
                password = modal.Password,
                correo = modal.Correo,
                nombre = modal.Nombre,
                appaterno = modal.ApPaterno,
                apmaterno = modal.ApMaterno,
                rfc = modal.RFC,
                homoclave = modal.Homoclave,
                municipio = modal.Municipio,
                rol = modal.Rol
            };

            _context.Usuarios.Add(usr);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Usuario registrado correctamente.";
            return RedirectToAction("Usuarios");
        }

        var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        TempData["Errores"] = errores;
        return RedirectToAction("Usuarios");
    }

    [HttpPost]
    public async Task<IActionResult> Eliminar(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
        {
            TempData["Errores"] = new List<string> { "Usuario no encontrado." };
            return RedirectToAction("Usuarios");
        }

        try
        {
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = $"El usuario '{usuario.usuarioapp}' ha sido eliminado correctamente.";
            return RedirectToAction("Usuarios");
        }
        catch (Exception ex)
        {
            TempData["Errores"] = new List<string> { "Ocurrió un error al intentar eliminar el usuario.", ex.Message };
            return RedirectToAction("Usuarios");
        }
    }
}
