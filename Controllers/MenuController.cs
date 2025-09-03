using Microsoft.AspNetCore.Mvc;

namespace MAC.Controllers
{
    public class MenuController : BaseController
    {
        public IActionResult Index()
        {
            ViewBag.CurrentController = "Menu";
            ViewBag.CurrentAction = "Index";
            return View();
        }
    }
}
