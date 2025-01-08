using Dapper;
using Microsoft.Data.SqlClient;
using Project_PicturesGalleryPlatform.Models;

namespace Project_PicturesGalleryPlatform.Repositories.ImageRepository
{
    public class ImageRepository : IImageRepository
    {
        //private const string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Database=Images;User ID=Test;Password=12345678;Trusted_Connection=True";
        // 資料庫連接字串
        //private readonly string ConnectionString = "Server=tcp:test241214.database.windows.net,1433;Initial Catalog=Test;Persist Security Info=False;User ID=test;Password=Abcd1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private readonly string ConnectionString = "Server=tcp:group1project.database.windows.net,1433;Initial Catalog=PicturesGallery;Persist Security Info=False;User ID=manager;Password=Abcd1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        // 執行 SQL 查詢並返回結果
        private List<ImageDetails> ExecuteQuery(string sqlQuery, object parameters = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<ImageDetails>(sqlQuery, parameters).ToList() ?? new List<ImageDetails>();  // 執行查詢並返回結果，若無結果則返回空列表
            }
        }

        // 生成分頁查詢語句
        private string GetPagedQuery(string baseQuery, int page, int pageSize)
        {
            return $"{baseQuery} ORDER BY id OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
        }

        // 獲取隨機圖片（最大 100 張）
        public List<ImageDetails> GetRandomImages()
        {
            var sqlQuery = "SELECT TOP 100 * FROM Pictures ORDER BY NEWID()"; // 使用 NEWID() 隨機排列圖片
            return ExecuteQuery(sqlQuery);
        }

        // 根據關鍵字搜尋圖片，支持分頁
        public List<ImageDetails> SearchImagesByKeyword(string keyword, int page, int pageSize)
        {
            var sqlQuery = GetPagedQuery("SELECT * FROM Pictures WHERE title LIKE @SearchKeyword", page, pageSize);
            return ExecuteQuery(sqlQuery, new { SearchKeyword = $"%{keyword}%", Offset = page * pageSize, PageSize = pageSize });
        }

        // 根據圖片 ID 獲取與其主題相同的相關圖片
        public List<ImageDetails> GetRelatedImagesById(int id)
        {
            var sqlQuery = "SELECT * FROM Pictures WHERE id != @Id AND theme = (SELECT theme FROM Pictures WHERE id = @Id)";
            return ExecuteQuery(sqlQuery, new { Id = id });
        }

        // 根據 ID 列表獲取圖片，支持分頁
        public List<ImageDetails> GetImagesByIds(List<int> ids, int page, int pageSize)
        {
            var sqlQuery = GetPagedQuery("SELECT * FROM Pictures WHERE id IN @Ids", page, pageSize);
            return ExecuteQuery(sqlQuery, new { Ids = ids, Offset = page * pageSize, PageSize = pageSize });
        }

        // 根據標籤（tag）獲取圖片，支持分頁
        public List<ImageDetails> GetImagesByTag(string tag, int page, int pageSize)
        {
            var sqlQuery = GetPagedQuery("SELECT * FROM Pictures WHERE tag = @Tag", page, pageSize);
            return ExecuteQuery(sqlQuery, new { Tag = tag, Offset = page * pageSize, PageSize = pageSize });
        }

        // 根據圖片 ID 獲取單張圖片
        public List<ImageDetails> GetImagesById(int id)
        {
            var sqlQuery = "SELECT * FROM Pictures WHERE id = @id";
            return ExecuteQuery(sqlQuery, new { id });
        }
    }
}
