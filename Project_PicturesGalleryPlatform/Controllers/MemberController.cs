using Microsoft.AspNetCore.Mvc;
using Project_PicturesGalleryPlatform.Models.UploadModel;

namespace Project_PicturesGalleryPlatform.Controllers
{
    public partial class MemberController : Controller
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



    /// <summary>
    /// 我把我和你的部分分開來
    /// by 永旭
    /// </summary>
    public partial class MemberController : Controller
    {
        [HttpPost]
        public IActionResult Upload(pictureInformation data)
        {
            // 檢查是否登入
            //if (Request.Cookies.ContainsKey("UserAccount")
            bool conditionTemp = Request.Cookies.ContainsKey("UserAccount");
            conditionTemp = false;// 測試用，之後要刪掉 line 63
            Console.WriteLine("MemberController.Upload() 測試中 conditionTemp: {0}", conditionTemp);
            if (conditionTemp) // 已登入
            {
                //var userAccount = Request.Cookies["UserAccount"];
                //ViewBag.User = userAccount;
                UserUpload userUpload = new();
                var checkResult = userUpload.DataCheck(data);
                if (checkResult != null)// 資料不完整
                {
                    Console.WriteLine("Upload資料安全驗證失敗");
                    _AlertSetting("上傳失敗:\n" + checkResult + "請再嘗試一遍");
                    return View();
                }
                else// 資料完整
                {
                    userUpload.SaveUploadedFile(data);
                    if (userUpload.ImageDataToDB(data))
                    {
                        _AlertSetting("上傳成功!");
                        return RedirectToAction("Member");
                    }
                    else
                    {
                        _AlertSetting("伺服器端錯誤，上傳失敗");
                        return RedirectToAction("Upload");
                    }
                }
            }
            else// 未登入
            {// *測試中
                Console.WriteLine("請用戶先登入");
                _AlertSetting("請先登入", "Login", "Login");
                //return RedirectToAction("Login", "Login");
                return RedirectToAction("Upload_transition");
            }
        }



        /// <summary>
        /// 設定前端彈出視窗的訊息
        /// </summary>
        /// <param name="str"></param>
        private void _AlertSetting(object str)
        {
            TempData["uploadFeedback"] = str;
            TempData["triggerAlert"] = true;
        }
        private void _AlertSetting(object str, string move, string con)
        {
            TempData["uploadFeedback"] = str;
            TempData["triggerAlert"] = true;
            TempData["move"] = move;
            TempData["controller"] = con;
        }



        public IActionResult Upload()
        {
            // *檢查是否登入
            return View();
        }



        public IActionResult Upload_transition()
        {
            return View();
        }

    }

}
