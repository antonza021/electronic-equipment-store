using Microsoft.AspNetCore.Mvc;

namespace electronic_equipment_store.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult AunthError()
        {
            return View();
        }
        public IActionResult NotEnoughRights()
        {
            return View();
        }
    }
}
