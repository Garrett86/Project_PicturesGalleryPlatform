using Microsoft.AspNetCore.Mvc;
using Project_PicturesGalleryPlatform.Services;
using System.Diagnostics;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

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

        //點擊單照片
        public IActionResult PictureInfo(int id)
        {
            var pictures = _imageService.GetAccountsById(id);
            ViewData["picture"] = pictures;
            return View();
        }

        //搜尋類別
        public IActionResult SearchTag(String tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                return View("Index", _imageService.GetRandomImages());
            }
            ViewData["tag"] = tag;
            var images = _imageService.GetAccountsByTag(tag);
            return View("../Page/Pagination");
        }
        [HttpPost]
        public IActionResult GetImagesByFile(IFormFile uploadfile)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", uploadfile.FileName);
            string output;
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = @"C:\ProgramData\anaconda3\python.exe",
                Arguments = @"C:\Users\USER\Desktop\image_similarity_search.py" + " " + filePath,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(startInfo))
            {
                using (System.IO.StreamReader reader = process.StandardOutput)
                {
                    output = reader.ReadToEnd().Split("\r")[0];
                }
            }
            var images = _imageService.GetImagesByIds(output);
            return View("../Page/Result");
        }
    }
}
