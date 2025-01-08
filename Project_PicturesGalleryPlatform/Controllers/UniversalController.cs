using Microsoft.AspNetCore.Mvc;

namespace Project_PicturesGalleryPlatform.Controllers
{
    public class UniversalController : Controller
    {
        public IActionResult TransitionPage()
        {
            return View();
        }
    }
}
