using Microsoft.AspNetCore.Mvc;

namespace Project_PicturesGalleryPlatform.Controllers
{
    public class PeopleController : Controller
    {
        public IActionResult People()
        {
            return View();
        }
    }
}
