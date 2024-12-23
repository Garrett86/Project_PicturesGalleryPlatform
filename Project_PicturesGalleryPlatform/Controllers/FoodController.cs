using Microsoft.AspNetCore.Mvc;

namespace Project_PicturesGalleryPlatform.Controllers
{
    public class FoodController : Controller
    {
        public IActionResult Food()
        {
            return View();
        }
    }
}
