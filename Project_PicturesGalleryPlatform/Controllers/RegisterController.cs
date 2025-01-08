using Microsoft.AspNetCore.Mvc;
using Project_PicturesGalleryPlatform.Models;

namespace Project_PicturesGalleryPlatform.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegisterController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Member member)
        {
            // 先前有登入, 先強制 Logout
            if (Request.Cookies.ContainsKey("UserAccount"))
            {
                Response.Cookies.Delete("UserAccount");
            }
            //
            if (!ModelState.IsValid)
            {
                return View(member);
            }

            // 檢查帳號是否重複
            if (_context.Members.Any(m => m.account == member.account))
            {
                ModelState.AddModelError("account", "帳號已被使用，請選擇其他帳號。");
                return View(member);
            }

            // 儲存到資料庫
            member.initDate = DateTime.Now;
            _context.Members.Add(member);
            _context.SaveChanges();

            // 顯示註冊成功訊息
            TempData["SuccessMessage"] = $"{member.account} 註冊成功! 請登入";

            return RedirectToAction("Index", "Home");
        }
    }
}
