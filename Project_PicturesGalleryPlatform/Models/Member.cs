using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_PicturesGalleryPlatform.Models
{
    public class Member
    {
        [Key]
        [Required(ErrorMessage = "帳號為必填項目")]
        public string account { get; set; }

        [Required(ErrorMessage = "密碼為必填項目")]
        public string password { get; set; }

        [Required(ErrorMessage = "姓名為必填項目")]
        public string name { get; set; }

        public string? email { get; set; }

        public DateTime initDate { get; set; }

        // 新增 passwordConfirm 屬性 (僅用於前端驗證)
        [NotMapped]
        [Compare("password", ErrorMessage = "密碼與確認密碼不一致")]
        public string passwordConfirm { get; set; }
    }
}

