using Microsoft.AspNetCore.Mvc;
using Project_PicturesGalleryPlatform.Models.AIPicturesModels;
using Microsoft.EntityFrameworkCore;
using Project_PicturesGalleryPlatform.Models;
using Project_PicturesGalleryPlatform.Services.ImageAnalysisService;
using Project_PicturesGalleryPlatform.Services.ImageService;
using Project_PicturesGalleryPlatform.Services.MyFavoritesService;
using System.Diagnostics;
using System.Text.Json;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace Project_PicturesGalleryPlatform.Controllers
{
    public class PageController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IImageService _imageService;
        private readonly IMyFavoritesService _myFavoritesService;
        private readonly IImageAnalysisService _imageAnalysisService;
        private readonly ApplicationDbContext _dbContext;

        public PageController(ILogger<HomeController> logger, IImageService imageService, IImageAnalysisService imageAnalysisService, ApplicationDbContext dbContext, IMyFavoritesService myFavoritesService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _imageService = imageService;
            _imageAnalysisService = imageAnalysisService;
            _myFavoritesService = myFavoritesService;
        }
        public IActionResult Result()
        {
            return View();
        }
        //點擊單照片
        public IActionResult PictureInfo(int id)
        {
            var pictures = _imageService.GetImagesById(id);
            ViewData["picture"] = pictures;
            return View();
        }

        //[HttpPost]
        //public IActionResult GetImagesByFile(IFormFile uploadfile)
        //{
        //    var images =_imageAnalysisService.FindSimilarImagesByImage(uploadfile);
        //    return View("../Page/Pagination");
        //}
        [HttpPost]
        public IActionResult ToggleImageLikeStatus(int id, bool isFavorited)
        {
            // 從 Cookie 讀取用戶帳號，若帳號不存在則返回失敗
            String? userAccount = Request.Cookies["UserAccount"];
            if (string.IsNullOrEmpty(userAccount))
                return Json(new { success = false });

            // 根據 isFavorited 狀態執行相應的收藏操作
            if (isFavorited)
                _myFavoritesService.RemoveFavorite(userAccount, id);
            else
                _myFavoritesService.AddFavorite(userAccount, id);

            return Json(new { success = true });  // 返回操作結果
        }

        /// <summary>
        /// 生成AI圖片，用於ajax請求
        /// </summary>
        /// <returns>生成的圖片資訊</returns>
        public JsonResult GenPic(string keyword)
        {
            // test當前工作目錄
            Console.WriteLine("Page//GenPic keyword: " + keyword);
            PYProcess_AI genPic = new PYProcess_AI(keyword, 1);// *測試 僅生成一張圖
            var picturesData = genPic.Generate();
            // 判斷是否成功生成圖片
            if (picturesData == null)
            {
                Console.WriteLine("生成圖片失敗");
                return Json(new { status = "fail", message = "生成圖片失敗" });
            }
            else
            {
                Console.WriteLine("生成圖片成功");
                return Json(new { status = "success", message = "生成圖片成功", keyword_AI = keyword, pictures = picturesData });
            }
        }
    }
}
