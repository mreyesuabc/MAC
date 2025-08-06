using Microsoft.AspNetCore.Mvc;

namespace MAC.Controllers
{
    public class MenuController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
