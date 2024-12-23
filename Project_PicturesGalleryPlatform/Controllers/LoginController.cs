using Microsoft.AspNetCore.Mvc;

namespace Project_PicturesGalleryPlatform.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
