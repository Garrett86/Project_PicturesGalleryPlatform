using System.ComponentModel.DataAnnotations;

namespace Project_PicturesGalleryPlatform.Models.UploadModel
{
    public class pictureInformation
    {

        [Required(ErrorMessage ="*標題欄位不能為空白")]
        public string title {  get; set; }
        
        [Required(ErrorMessage ="*tag欄位不能為空白")]
        public string tag { get; set; }

        [Required(ErrorMessage ="*必須選擇圖片檔")]
        public IFormFile file { get; set; }

    }
}
