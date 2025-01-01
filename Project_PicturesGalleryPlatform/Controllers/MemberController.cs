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
    /// MemberController_上傳圖片功能部分<para>
    /// 我把我和你的部分分開來 by 永旭</para>
    /// </summary>
    public partial class MemberController : Controller
    {
        [HttpPost]
        public IActionResult Upload(pictureInformation data)
        {
            // 檢查是否登入
            bool loginCon = Request.Cookies.ContainsKey("UserAccount");
            loginCon = true;// 測試用，之後要刪掉 
            Console.WriteLine("MemberController.Upload() 測試中 conditionTemp: {0}", loginCon);
            if (loginCon) // 已登入
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
                    if (userUpload.ImageDataToDB(data))// 上傳成功
                    {
                        _AlertSetting("上傳成功!", "Member", "Member");
                        return RedirectToAction("Upload_transition");
                    }
                    else// 上傳失敗
                    {
                        _AlertSetting("伺服器端錯誤，上傳失敗");
                        return View();
                    }
                }
            }
            else// 未登入
            {// *測試中 暫時封閉
                Console.WriteLine("請用戶先登入");
                _AlertSetting("請先登入", "Login", "Login");
                //return RedirectToAction("Login", "Login");
                return RedirectToAction("Upload_transition");
            }
        }



        /// <summary>
        /// 設定前端彈出視窗的訊息(不設定重新導向)
        /// </summary>
        /// <param name="str">顯示訊息</param>
        private void _AlertSetting(object str)
        {
            TempData["uploadFeedback"] = str;
            TempData["triggerAlert"] = true;
        }
        /// <summary>
        /// 觸發、設定前端彈出視窗的訊息，設定重新導向
        /// </summary>
        /// <param name="str">顯示訊息</param>
        /// <param name="action">導向action</param>
        /// <param name="con">導向controller</param>
        private void _AlertSetting(object str, string action, string con)
        {
            TempData["uploadFeedback"] = str;
            TempData["triggerAlert"] = true;
            TempData["action"] = action;
            TempData["controller"] = con;
        }



        public IActionResult Upload()
        {
            bool loginCon = Request.Cookies.ContainsKey("UserAccount");
            Console.WriteLine("測試 輸入1為已登入狀態 / 輸入其他按鍵為未登入狀態");
            loginCon = Console.ReadLine()=="1";// 測試用，之後要刪掉 
            Console.WriteLine("MemberController.Upload() 測試中 conditionTemp: {0}", loginCon);
            if (loginCon) // 已登入
            {
                TempData["triggerAlert"] = false;
                return View();
            }
            else// 未登入
            {
                Console.WriteLine("請用戶先登入");
                _AlertSetting("請先登入", "Login", "Login");
                //return RedirectToAction("Login", "Login");
                return RedirectToAction("Upload_transition");
            }
        }



        public IActionResult Upload_transition()
        {
            return View();
        }

    }

}
