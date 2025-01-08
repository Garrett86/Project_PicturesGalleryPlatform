using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_PicturesGalleryPlatform.Models;
using Project_PicturesGalleryPlatform.Models.AIPicturesModels;
using Project_PicturesGalleryPlatform.Repositories.IRatingService;
using Project_PicturesGalleryPlatform.Services.ImageService;
using Project_PicturesGalleryPlatform.Services.MyFavoritesService;
using System.Security.Claims;

namespace Project_PicturesGalleryPlatform.Controllers
{
    
    public class SinglePicController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IImageService _imageService;
        //private readonly IMyFavoritesService _myFavoritesService;
        protected readonly IRatingService _ratingService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;

        public SinglePicController(ILogger<HomeController> logger, IImageService imageService, IRatingService ratingService, IWebHostEnvironment webHostEnvironment, ApplicationDbContext context)
        {
            _logger = logger;
            _imageService = imageService;
            //_myFavoritesService = myFavoritesService;
            _ratingService = ratingService;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        public IActionResult SinglePic(int id)
        {

            ViewData["user"] = Request.Cookies["UserAccount"];

            var pictures = _imageService.GetImagesById(id);
            if (pictures == null)
            {
                return NotFound();
            }
            ViewData["picture"] = pictures;
            return View();
        }

        // 假設有一個方法來根據用戶帳號獲取用戶資料


        // 提交評分
        [HttpPost]
        public async Task<IActionResult> SubmitRating([FromBody] SinglePicViewModel model)
        {
            // 檢查用戶是否登入
            // 從 Cookie 中讀取 "UserAccount"
            var userAccount = Request.Cookies["UserAccount"];
            if (string.IsNullOrEmpty(userAccount))
            {
                return Json(new { success = false, message = "用戶尚未登入" });
            }
            // 查詢資料庫來獲取用戶名稱
            var user = await _context.Members.FirstOrDefaultAsync(m => m.account == userAccount);
            if (user == null)
            {
                return Json(new { success = false, message = "找不到用戶資料" });
            }

            var username = user.account;

            // 驗證評分數據的有效性
            if (model == null || model.ProductId <= 0 || model.Rating < 1 || model.Rating > 5)
            {
                return Json(new { success = false, message = "無效的評分數據。" });
            }

            try
            {
                // 檢查用戶是否已對該圖片評分
                bool isAlreadyRated = _ratingService.IsPictureInpictureScore(username, model.ProductId);

                bool result = false;

                if (isAlreadyRated)
                {
                    // 如果已經評分，執行更新操作
                    result = _ratingService.UpdateRating(model.ProductId, username, model.Rating);
                }
                else
                {
                    // 如果未評分，執行新增評分操作
                    result = _ratingService.AddpictureScore(model.ProductId, username, model.Rating);
                }

                if (result)
                {
                    return Json(new { success = true, message = "評分成功！" });
                }
                else
                {
                    return Json(new { success = false, message = "儲存評分失敗，請稍後再試。" });
                }

            }
            catch (Exception ex)
            {
                // 記錄錯誤
                _logger.LogError(ex, "評分過程中發生錯誤");

                // 回傳 500 錯誤，並附帶錯誤訊息
                return StatusCode(500, new { success = false, message = "伺服器內部錯誤，請稍後再試。" });
            }
        }

        public class SinglePicViewModel
        {
            public int ProductId { get; set; }
            public byte Rating { get; set; }
        }

        public IActionResult SinglePic_AI(AIPicData aiPicData, string keyword)
        {
            Console.WriteLine("Single//Single_AI 接收參數: {0}&{1}", aiPicData, keyword);
            // 處理數據
            DataTransformer dataTransformer = new DataTransformer(aiPicData);
            ImageDetails imageDetail = dataTransformer.Transform(keyword);
            //ViewData["imageDetail"] = imageDetail;
            return View(imageDetail);
        }

        public ActionResult DownloadFile(string picId)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;

            // 拼接出檔案的物理路徑
            string filePath = Path.Combine(webRootPath, "images2", picId + ".webp");
            string fileName = picId + ".jpg";
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, "application/octet-stream", fileName);
        }

    }
}

