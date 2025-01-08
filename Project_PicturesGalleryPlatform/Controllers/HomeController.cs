using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Project_PicturesGalleryPlatform.Models;
using Project_PicturesGalleryPlatform.Services.ImageService;
using Project_PicturesGalleryPlatform.Models.AIPicturesModels;

using Project_PicturesGalleryPlatform.Repositories.IRatingService;

namespace Project_PicturesGalleryPlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IImageService _imageService;
        private readonly IRatingService _ratingService;

        public HomeController(ILogger<HomeController> logger, IImageService imageService, IRatingService ratingService)
        {
            _logger = logger;
            _imageService = imageService;
            _ratingService = ratingService;
        }


        public IActionResult Index()
        {
            if (Request.Cookies.ContainsKey("UserAccount"))
            {
                ViewBag.User = Request.Cookies["UserAccount"]; // �� Cookies ȡ��ʹ�������Q
            }
            else
            {
                ViewBag.User = null; // δ����r���O�Þ� null
            }
            // 載入資料庫評分表單
            var totalScores = _ratingService.GetUserTotalScore();
            ViewData["TotalScores"] = totalScores;
            return View();
        }



        [HttpPost]
        public IActionResult AIPictures(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {// �Ȥ������ŭ�
                TempData["feedbackMessage"] = "�п�J���Ī�����r�C";
                TempData["action"] = "Index";
                TempData["controller"] = "Home";
                return RedirectToAction("TransitionPage", "Universal");
            }
            var temps = (keyword.Trim()).Split(" ");// bug������01/07
            string newKeyword = "";
            foreach (var temp in temps)
            {
                newKeyword += temp;
            }
            Console.WriteLine("Home//AIPictures����&�B�z��keyword: {0}", newKeyword);
            TempData["keyword_AI"] = newKeyword;
            TempData.Keep("keyword_AI");
            return View("~/Views/Page/Result_AI.cshtml");// �ݴ���
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Logout()
        {
            if (Request.Cookies.ContainsKey("UserAccount"))
            {
                Response.Cookies.Delete("UserAccount"); // �h�� UserAccount �� Cookie
            }
            return RedirectToAction("Index", "Home"); // �ǳ��ጧ����?
        }
    }
}
