using Microsoft.AspNetCore.Mvc;

namespace Project_PicturesGalleryPlatform.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult Member()
        {
            return View();
        }

        public IActionResult MemberModify()
        {
            return View();
        }
    }

}
