using Dapper;
using Microsoft.Data.SqlClient;

namespace Project_PicturesGalleryPlatform.Repositories.MyFavoritesRepository
{
    public class MyFavoritesRepository : IMyFavoritesRepository
    {
        // 資料庫連接字串
        private readonly string ConnectionString = "Server=tcp:test250108.database.windows.net,1433;Initial Catalog=PicturesGallery;Persist Security Info=False;User ID=manager;Password=Abcd1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        // 執行非查詢 SQL
        private void ExecuteNonQuery(string sqlQuery, object parameters = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute(sqlQuery, parameters);
            }
        }

        // 新增收藏
        public void AddFavorite(string userAccount, int pictureId)
        {
            var sqlQuery = "INSERT INTO favorite (userAccount, pictureId) VALUES (@UserAccount, @PictureId)";
            ExecuteNonQuery(sqlQuery, new { UserAccount = userAccount, PictureId = pictureId });
        }

        // 移除收藏
        public void RemoveFavorite(string userAccount, int pictureId)
        {
            var sqlQuery = "DELETE FROM favorite WHERE userAccount = @UserAccount AND pictureId = @PictureId";
            ExecuteNonQuery(sqlQuery, new { UserAccount = userAccount, PictureId = pictureId });
        }

        // 取得使用者收藏的圖片 ID 列表
        public List<int> GetUserFavoritePictureIds(string userAccount)
        {
            var sqlQuery = "SELECT pictureId FROM favorite WHERE userAccount = @UserAccount";
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<int>(sqlQuery, new { UserAccount = userAccount }).ToList() ?? new List<int>();
            }
        }
    }
}
