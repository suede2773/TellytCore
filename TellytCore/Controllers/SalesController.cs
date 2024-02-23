using Microsoft.AspNetCore.Mvc;

namespace TellytCore.Controllers
{
    public class SalesController : Controller
    {
        // GET: Sales
        public IActionResult Index()
        {
            return View();
        }
    }
}
