using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;

namespace Project_PicturesGalleryPlatform.Models.UploadModel
{
    public class UserUpload
    {
        private static readonly string connectionString =
            "Server=tcp:group1project.database.windows.net,1433;Initial Catalog=PicturesGallery;Persist Security Info=False;" +
            "User ID=manager;Password=Abcd1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        string? foldpath;
        string? OriPath;
        int PicWidth;
        int PicHeight;



        /// <summary>
        /// 用戶端回傳之資料安全驗證
        /// </summary>
        /// <returns>返回包含所有錯誤訊息的List</returns>
        public string DataCheck(pictureInformation data)
        {
            
            ValidationContext context = new (data, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(data, context, validationResults, true);
            string result = null;
            if (isValid) { Console.WriteLine("用戶資料有效"); } 
            else 
            { 
                foreach (var validationResult in validationResults) 
                {
                    result += validationResult.ErrorMessage + "\n";
                    Console.WriteLine(validationResult.ErrorMessage); 
                }
            }
            return result;
        }



        /// <summary>
        /// 圖片檔寫入
        /// </summary>
        /// <returns></returns>
        public async Task SaveUploadedFile(pictureInformation data)
        {
            IFormFile file = data.file;
            //// 取得檔名
            var fileName = Path.GetFileName(file.FileName);
            //// 設定路徑
            this.foldpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploaded_pictures");
            if (!Directory.Exists(foldpath)) //// 確保路徑存在
            {
                Directory.CreateDirectory(foldpath);
            }
            this.OriPath = Path.Combine(foldpath, fileName);

            using (var stream = new FileStream(this.OriPath, FileMode.Create)) //// 儲存文件
            {
                await file.CopyToAsync(stream);
            }
            //// 取得圖片長寬
            using (Image image = Image.FromFile(this.OriPath))
            {
                this.PicWidth = image.Width;
                this.PicHeight = image.Height;
            }
        }




        /// <summary>
        /// 圖片資訊寫入資料庫&圖片檔改名
        /// </summary>
        /// <returns>返回是否成功</returns>
        public bool ImageDataToDB(pictureInformation data)
        {
            int pictureId = 0;
            using (SqlConnection sqlConnection = new(connectionString))
            {
                try
                {
                    //// 先取得ID(取得表內的MAXid 然後+1)
                    sqlConnection.Open();
                    string commandString = @"SELECT MAX(id) FROM Pictures";
                    using (SqlCommand sqlCommand = new(commandString, sqlConnection))
                    {
                        object result = sqlCommand.ExecuteScalar();
                        if (result == null || result == DBNull.Value) { pictureId = 1; }
                        else { pictureId = Convert.ToInt32(result) + 1; }
                    }
                    //// 再將資料插入
                    
                    goto skipInsert;// *測試用，暫時以下封閉
                    commandString = @"INSERT INTO Pictures(id, title, tag, width, height) VALUES(@id, @title, @tag, @width, @height)";
                    using (SqlCommand sqlCommand = new(commandString, sqlConnection))
                    {
                        sqlCommand.Parameters.Add(new SqlParameter("@id", pictureId));
                        sqlCommand.Parameters.Add(new SqlParameter("@title", data.title));
                        sqlCommand.Parameters.Add(new SqlParameter("@tag", data.tag));
                        sqlCommand.Parameters.Add(new SqlParameter("@width", this.PicWidth));
                        sqlCommand.Parameters.Add(new SqlParameter("@height", this.PicHeight));

                        sqlCommand.ExecuteNonQuery();
                    }
                    skipInsert:
                    Console.WriteLine("測試中，跳過資料庫寫入");

                    //// *圖片移動至正確路徑(我們需要正式決定一下圖片儲存的位置
                    //// 將圖片名稱改以ID命名
                    var newpath = Path.Combine(this.foldpath, pictureId + ".jpg");//// 要修改路徑、副檔名
                    File.Move(this.OriPath, newpath);
                    return true;
                }
                catch (SqlException ex)
                {
                    //// 處理SQL異常
                    Console.WriteLine("SQL Exception: " + ex.Message);
                    return false;
                }
                catch (Exception ex)
                {   //// 處理一般異常
                    Console.WriteLine("General Exception: " + ex.Message);
                    return false;
                }
            }
        }
    }
}
