using Microsoft.AspNetCore.Mvc;

namespace BlazorMarkDownAppJwt.Server.Controllers
{
    public class MarkDownController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
