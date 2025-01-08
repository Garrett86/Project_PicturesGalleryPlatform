using Microsoft.AspNetCore.Mvc;
using Project_PicturesGalleryPlatform.Models.UploadModel;
using Microsoft.Extensions.Configuration;
using Project_PicturesGalleryPlatform.Models;
using System.Linq;

namespace Project_PicturesGalleryPlatform.Controllers
{
    public partial class MemberController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;

        public MemberController(IConfiguration configuration, ApplicationDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        // 顯示會員資料
        public IActionResult Member()
        {
            // 尝试获取会员对象
            Member member = null;

            if (Request.Cookies.ContainsKey("UserAccount")) // 检查是否存在有效的 Cookie
            {
                string userAccount = Request.Cookies["UserAccount"]; // 获取 UserAccount 的值
                member = _dbContext.Members.FirstOrDefault(m => m.account == userAccount); // 从数据库查询对应的会员信息
            }

            if (member == null) // 若会员为空，传递一个空对象
            {
                member = new Member
                {
                    account = "", // 默认值为空字符串
                    name = "",
                    email = ""
                };
            }

            return View("Member", member); // 返回视图，无论是否找到会员都传递 Member 模型
        }


        // 進入修改會員資料頁面
        public IActionResult MemberModify()
        {

            if (Request.Cookies.ContainsKey("UserAccount"))
            {
                string userAccount = Request.Cookies["UserAccount"];
                var member = _dbContext.Members.FirstOrDefault(m => m.account == userAccount);

                if (member != null)
                {
                    return View("MemberModify", member);
                }
            }

            // 若無法找到會員或沒有有效的 Cookie，返回首頁或其他提示頁
            return RedirectToAction("Index", "Home");
        }

        // 儲存修改後的會員資料
        [HttpPost]
        public IActionResult SaveModifiedMember(Member modifiedMember)
        {
            if (ModelState.IsValid)
            {
                var member = _dbContext.Members.FirstOrDefault(m => m.account == modifiedMember.account);

                if (member != null)
                {
                    member.name = modifiedMember.name;
                    member.email = modifiedMember.email;

                    // 檢查密碼一致性
                    if (!string.IsNullOrEmpty(modifiedMember.password) &&
                        modifiedMember.password == modifiedMember.passwordConfirm)
                    {
                        member.password = modifiedMember.password;
                    }
                    else if (!string.IsNullOrEmpty(modifiedMember.password))
                    {
                        ModelState.AddModelError("passwordConfirm", "密碼與確認密碼不一致");
                        return View("MemberModify", modifiedMember);
                    }

                    _dbContext.SaveChanges();
                    
                    // 先前有登入, 先強制 Logout
                    if (Request.Cookies.ContainsKey("UserAccount"))
                                {
                                 Response.Cookies.Delete("UserAccount");
                                }
                    return RedirectToAction("Index", "Home");
                }
            }

            // 如果數據無效，返回修改頁面並提示錯誤
            return View("MemberModify", modifiedMember);
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
            //loginCon = true;// 測試用，之後要刪掉 
            //Console.WriteLine("MemberController.Upload() 測試中 conditionTemp: {0}", loginCon);
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
            {// 開放
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
            //Console.WriteLine("測試 輸入1為已登入狀態 / 輸入其他按鍵為未登入狀態");
            //loginCon = Console.ReadLine()=="1";// 測試用，之後要刪掉 
            //Console.WriteLine("MemberController.Upload() 測試中 conditionTemp: {0}", loginCon);
            if (loginCon) // 已登入
            {
                TempData["triggerAlert"] = false;
                return View();
            }
            else// 未登入
            {
                Console.WriteLine("請用戶先登入");
                _AlertSetting("請先登入", "Login", "Login");
                return RedirectToAction("Upload_transition");
            }
        }
        public IActionResult Favorites()
        {
            String? userAccount = Request.Cookies["UserAccount"];

            if (string.IsNullOrEmpty(userAccount))
                return View("../Login/Login");
            return RedirectToAction("MyFavorites", new { userAccount = userAccount });
        }

        public IActionResult MyFavorites()
        {
            return View();
        }


        public IActionResult Upload_transition()
        {
            return View();
        }

    }

}
