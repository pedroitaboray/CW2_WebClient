using Microsoft.AspNetCore.Mvc;

namespace CW2_WebClient.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
