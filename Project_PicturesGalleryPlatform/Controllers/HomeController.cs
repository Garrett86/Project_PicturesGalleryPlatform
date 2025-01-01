using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Project_PicturesGalleryPlatform.Models;
using Project_PicturesGalleryPlatform.Services;

namespace Project_PicturesGalleryPlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IImageService _imageService;

        public HomeController(ILogger<HomeController> logger, IImageService imageService)
        {
            _logger = logger;
            _imageService = imageService;
        }

        public IActionResult Index()
        {
            //var pictures = _imageService.GetAccountsById(1);
            //ViewData["picture"] = pictures;
            //return View("../Page/PictureInfo");
            return View();
        }

        [HttpPost]
        public IActionResult SearchImages(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                ViewData["ErrorMessage"] = "請輸入有效的關鍵字。";
                return View("Index", _imageService.GetRandomImages());
            }

            ViewData["keyword"] = keyword;
            var images = _imageService.GetImagesByKeyword(keyword);
            return View("../Page/Result");
            

        }



        public JsonResult GetImagesByPage(int page, int pageSize)
        {
            if (page < 0 || pageSize <= 0)
            {
                return Json(new { error = "無效的頁面或每頁大小參數。" });
            }

            var images = _imageService.GetImagesByPage(page, pageSize);
            return Json(images);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
