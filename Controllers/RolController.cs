using AspNetCoreGeneratedDocument;
using MAC.Models;
using MAC.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Reflection.Metadata;
using System.Xml.Linq;

public class RolController : Controller
{
    private readonly MACDbContext _context;

    public RolController(MACDbContext context)
    {
        _context = context;
    }

    // Acción para mostrar la lista de usuarios del sistema
    public IActionResult IndexRol()
    {

        ViewBag.NombreCompleto = HttpContext.Session.GetString("NombreCompleto") ?? "Usuario";
        ViewBag.Roles = HttpContext.Session.GetString("Roles") ?? "";

        var listaRoles = _context.Rol.ToList();
        ViewBag.CurrentController = "Rol";
        ViewBag.CurrentAction = "IndexRol"; // o el nombre de la acción que estás usando
        return View(listaRoles);
    }
    public IActionResult EditRolParcial(int id)
    {
        var rol = _context.Rol.FirstOrDefault(u => u.Id == id);
        if (rol == null)
        {
            return NotFound();
        }
        return PartialView("_EditRol", rol);
    }


    [HttpPost]
    public IActionResult GuardarCambiosRol(Rol rol)
    {
        var rolDb = _context.Rol.FirstOrDefault(u => u.Id == rol.Id);
        if (rolDb == null)
        {
            return NotFound();
        }

        rolDb.descr = rol.descr;
        rolDb.activo = rol.activo;

        _context.SaveChanges();

        return Ok();
    }

[HttpPost]
    public async Task<IActionResult> AddRol(Rol rol)
    {
        if (ModelState.IsValid)
        {   
            
            _context.Rol.Add(rol);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Rol registrado correctamente.";
            ViewBag.CurrentController = "Rol";
            ViewBag.CurrentAction = "IndexRol";
            return RedirectToAction("Index");
        }
        var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        TempData["Errores"] = errores;
        //TempData["Errores"] = "Error al agregar el rol";
        ViewBag.CurrentController = "Rol";
        ViewBag.CurrentAction = "IndexRol";
        return RedirectToAction("Index");
    }

    public IActionResult Index()
    {
        var rol = _context.Rol.ToList();
        ViewBag.CurrentController = "Rol";
        ViewBag.CurrentAction = "IndexRol";
        return View("IndexRol", rol);
    }
    [HttpPost]
    public IActionResult ActualizarActivo(int id, int activo)
    {
        var rol = _context.Rol.FirstOrDefault(p => p.Id == id);
        if (rol == null)
        {
            return NotFound();
        }

        rol.activo = activo;
        _context.SaveChanges();

        return Ok();
    }

}

//using MAC.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;

//public class RolController : Controller
//{
//    private readonly MACDbContext _context;

//    public RolController(MACDbContext context)
//    {
//        _context = context;
//    }

//    // Mostrar roles existentes usando la vista IndexRol.cshtml
//    public IActionResult IndexRol()
//    {
//        var listaRoles = _context.Rol.ToList();
//        ViewBag.CurrentController = "Rol";
//        ViewBag.CurrentAction = "IndexRol"; 
//        return View(listaRoles);
//    }

//    [HttpPost]
//    public async Task<IActionResult> AddRol(Rol rol)
//    {
//        if (ModelState.IsValid)
//        {
//            _context.Rol.Add(rol);
//            await _context.SaveChangesAsync();
//            TempData["Mensaje"] = "Rol registrado correctamente.";
//            ViewBag.CurrentController = "Rol";
//            ViewBag.CurrentAction = "Roles";
//            return RedirectToAction("Index");
//        }

//        var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
//        TempData["Errores"] = errores;
//        ViewBag.CurrentController = "Rol";
//        ViewBag.CurrentAction = "Roles";
//        return RedirectToAction("Index");
//    }



//    [HttpPost]
//    public IActionResult ToggleActivo(int id)
//    {
//        var rol = _context.Rol.Find(id);
//        if (rol != null)
//        {
//            rol.activo = !rol.activo; // Invertir el valor booleano
//            _context.SaveChanges();
//        }
//        return RedirectToAction(nameof(Index)); // Asegúrate que esta vista exista
//    }

//}
