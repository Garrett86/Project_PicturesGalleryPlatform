using Microsoft.AspNetCore.Mvc;
using Project_PicturesGalleryPlatform.Models.Upload;

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

        [HttpPost]
        public IActionResult Upload(pictureInformation data)
        {
            bool conditionTemp = Request.Cookies.ContainsKey("UserAccount");
            conditionTemp = true;// 測試用，之後要刪掉
            if (conditionTemp)
            {
                //var userAccount = Request.Cookies["UserAccount"];
                //ViewBag.User = userAccount;
                UserUpload userUpload = new ();
                userUpload.SaveUploadedFile(data);
                if (userUpload.ImageDataToDB(data)) { return RedirectToAction("Member"); }
                else { return RedirectToAction("Upload"); }// *顯示上傳失敗
            }
            else
            {
                Console.WriteLine("請用戶先登入");
                //// 顯示"請先登入"提示
                return RedirectToAction("Login", "Login");
            }
            // 包成model 新增"上傳完成"提示

            
            // 條件判斷 上傳成功?
            return RedirectToAction("Upload");
        }
        public IActionResult Upload()
        {
            return View();
        }
    }

}
