
using Dapper;
using Microsoft.Data.SqlClient;
using Project_PicturesGalleryPlatform.Models;
using static System.Formats.Asn1.AsnWriter;

namespace Project_PicturesGalleryPlatform.Repositories.IRatingService
{

    public class RatingService : IRatingService
    {
        private readonly ILogger<RatingService> _logger;

        // 連接資料庫
        private readonly string ConnectionString = "Server=tcp:group1project.database.windows.net,1433;Initial Catalog=PicturesGallery;Persist Security Info=False;User ID=manager;Password=Abcd1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        //private readonly string ConnectionString = "Server=tcp:test241214.database.windows.net,1433;Initial Catalog=Test;Persist Security Info=False;User ID=test;Password=Abcd1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public RatingService(ILogger<RatingService> logger)
        {
            _logger = logger;
        }
        private void ExecuteNonQuery(string sqlQuery, object parameters = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute(sqlQuery, parameters);
            }
        }

        // 更新資料庫裡評分資料
        public bool UpdateRating(int productId, string username, byte score)
        {
            var sqlQuery = "UPDATE picturescore SET Score = @Score WHERE PictureId = @PictureId AND UserAccount = @UserAccount";
            try
            {
                // 執行更新操作
                ExecuteNonQuery(sqlQuery, new { PictureId = productId, UserAccount = username, Score = score });

                // 如果沒有異常發生，表示更新成功
                return true;
            }
            catch (Exception ex)
            {
                // 捕獲異常並記錄日誌（可選）
                _logger.LogError(ex, "更新評分時發生錯誤");

                // 如果捕獲到異常，表示更新失敗
                return false;
            }
        }

        // 寫入評分資料到資料庫
        public bool AddpictureScore(int productId, string username, byte score)
        {
            var sqlQuery = "INSERT INTO picturescore (PictureId, UserAccount, Score) VALUES (@PictureId, @UserAccount, @Score)";
            try
            {
                ExecuteNonQuery(sqlQuery, new { PictureId = productId, UserAccount = username, Score = score });
                return true;  // 如果成功，返回 true
            }
            catch (Exception)
            {
                return false;  // 如果發生異常，返回 false
            }
        }

        // 刪除
        public void RemovepictureScore(string userAccount, int pictureId)
        {
            var sqlQuery = "DELETE FROM picturescore WHERE userAccount = @UserAccount AND pictureId = @PictureId";
            ExecuteNonQuery(sqlQuery, new { UserAccount = userAccount, PictureId = pictureId });
        }

        public bool IsPictureInpictureScore(string userAccount, int pictureId)
        {
            var sqlQuery = "SELECT CASE WHEN EXISTS (SELECT 1 FROM favorite WHERE userAccount = @UserAccount AND pictureId = @PictureId) THEN 1 ELSE 0 END";
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.ExecuteScalar<int>(sqlQuery, new { UserAccount = userAccount, PictureId = pictureId }) == 1;
            }
        }

        // 讀取資料庫評分資料
        public List<int> GetUserpictureScorePictureIds(string userAccount)
        {
            var sqlQuery = "SELECT pictureId FROM picturescore WHERE userAccount = @UserAccount";
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<int>(sqlQuery, new { UserAccount = userAccount }).ToList();
            }
        }


        // 搜尋排行榜資料
        public List<PictureScore> GetUserTotalScore()
        {
            //var sqlQuery = "SELECT pictureId, score FROM picturescore WHERE score IS NOT NULL ORDER BY pictureId";
            var sqlQuery = "SELECT pictureId, SUM(CAST(score AS TINYINT)) AS Score FROM picturescore GROUP BY pictureId ORDER BY Score DESC";

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var result = connection.Query<PictureScore>(sqlQuery).ToList();

                // 確認結果
                //foreach (var score in result)
                //{
                //    Console.WriteLine($"PictureId: {score.PictureId}, TotalScore: {score.Score}");
                //}

                return result;
            }
        }
    }
}
