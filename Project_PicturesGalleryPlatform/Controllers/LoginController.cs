using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Project_PicturesGalleryPlatform.Models;

namespace Project_PicturesGalleryPlatform.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 顯示登入頁面
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // 處理登入邏輯
        [HttpPost]
        public IActionResult Login(string account, string password)
        {
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "帳號或密碼不能為空";
                return View();
            }

            // 查詢使用者
            var user = _context.Members.FirstOrDefault(m => m.account == account);

            if (user == null)
            {
                ViewBag.ErrorMessage = "帳戶不存在";
                return View();
            }

            if (user.password != password)
            {
                ViewBag.ErrorMessage = "密碼錯誤";
                return View();
            }

            // 設置登入 Cookie
            Response.Cookies.Append("UserAccount", account, new CookieOptions
            {
                //Expires = DateTimeOffset.Now.AddDays(7),
                HttpOnly = true
            });

            // 轉向成功頁面
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Success()
        {
            if (Request.Cookies.ContainsKey("UserAccount"))
            {
                ViewBag.User = Request.Cookies["UserAccount"];
                return View();
            }
            return RedirectToAction("Login");
        }

        

        
        public IActionResult Logout()
        {
            // 移除 UserAccount Cookie
            if (Request.Cookies.ContainsKey("UserAccount"))
            {
                Response.Cookies.Delete("UserAccount");
            }

            // 返回登入頁面
            return RedirectToAction("Index", "Home");
        }





    }
}