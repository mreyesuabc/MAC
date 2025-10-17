using AspNetCoreGeneratedDocument;
using MAC.Models;
using MAC.Models.ViewModels;
using MAC.Services;
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
    public async Task<IActionResult> MiPerfil()
    {

        var usuarioApp = HttpContext.Session.GetString("UsuarioApp");
        var usuario = await _context.Usuarios
            .Include(u => u.mpoInfo)
            .Include(u => u.rolInfo)
            .FirstOrDefaultAsync(u => u.usuarioapp == usuarioApp);

        if (usuario == null)
        {
            TempData["Errores"] = new List<string> { "Usuario no encontrado." };
            return RedirectToAction("Index", "Login");
        }

        var viewModel = new PerfilUsuarioViewModel
        {
            Usuario = usuario
        };
        ViewBag.CurrentController = "Usuario";
        ViewBag.CurrentAction = "MiPerfil";
        return View(viewModel);
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
        ViewBag.Roles = _context.Rol
                    .Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.descr })
                    .ToList();

        ViewBag.Municipios = _context.Municipios
                        .Select(m => new SelectListItem { Value = m.mpo.ToString(), Text = m.descr })
                        .ToList();
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
        usuarioDb.rol = usuario.rol;
        try
        {
            _context.SaveChanges();
            TempData["Mensaje"] = "Se actualizó información correctamente.";

        }
        catch (Exception ex)
        {
            TempData["Errores"] = "No fue posible actualizar la información del usuario."+ex;
        }
        return Ok();
    }
        [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarPassword(PerfilUsuarioViewModel model)
    {
        var usuarioId = HttpContext.Session.GetInt32("Id");
        var usuario = await _context.Usuarios.FindAsync(usuarioId);

        if (usuario == null)
        {
            TempData["Errores"] = "Usuario no encontrado.";
            return RedirectToAction("MiPerfil");
        }

        if (usuario.password == model.CambioPassword.Actual)
        {
            usuario.password = model.CambioPassword.Nueva;

            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Contraseña actualizada correctamente.";
            return RedirectToAction("MiPerfil");
        }
        else
        {
            TempData["Errores"] = "Contraseña actual es incorrecta.";
            return RedirectToAction("MiPerfil");
        }
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
        TempData["Errores"] = "No fue posible agregar al usuario";
        return RedirectToAction("Usuarios");
    }

    [HttpPost]
    public async Task<IActionResult> Eliminar(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
        {
            TempData["Errores"] = "Usuario no encontrado.";
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
            TempData["Errores"] ="Ocurrió un error al intentar eliminar el usuario."+ ex.Message;
            return RedirectToAction("Usuarios");
        }
    }
    [HttpGet]
    public async Task<IActionResult> ReiniciarPassword(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            TempData["Errores"] = "Usuario no encontrado.";
            return RedirectToAction("Usuarios");
        }

        // Generar nueva contraseña
        var nuevaPassword = usuario.usuarioapp;

        // Actualizar la contraseña
        usuario.password = nuevaPassword;
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
        TempData["Mensaje"] = $"La contraseña del usuario '{usuario.usuarioapp}' fue reiniciada como su usuario de aplicación, puede entrar a su perfil a cambairla.";
        return RedirectToAction("Usuarios");
    }

}
