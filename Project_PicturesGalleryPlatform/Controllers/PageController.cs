using Microsoft.AspNetCore.Mvc;
using Project_PicturesGalleryPlatform.Services;

namespace Project_PicturesGalleryPlatform.Controllers
{
    public class PageController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IImageService _imageService;

        public PageController(ILogger<HomeController> logger, IImageService imageService)
        {
            _logger = logger;
            _imageService = imageService;
        }

        public IActionResult PictureInfo(int id)
        {
            var pictures = _imageService.GetAccountsById(id);
            ViewData["picture"] = pictures;
            return View();
        }
    }
}
