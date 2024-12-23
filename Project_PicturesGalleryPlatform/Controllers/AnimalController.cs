using Microsoft.AspNetCore.Mvc;

namespace Project_PicturesGalleryPlatform.Controllers
{
    public class AnimalController : Controller
    {
        public IActionResult Animal()
        {
            return View();
        }
    }
}
