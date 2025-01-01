using Dapper;
using Microsoft.Data.SqlClient;
using Project_PicturesGalleryPlatform.Models;

namespace Project_PicturesGalleryPlatform.Repositories
{
    public class ImageDatabaseRepository : IImageRepository
    {
        //private const string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Database=Images;User ID=Test;Password=12345678;Trusted_Connection=True";
        private readonly string ConnectionString = "Server=tcp:test241214.database.windows.net,1433;Initial Catalog=Test;Persist Security Info=False;User ID=test;Password=Abcd1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        //private readonly string ConnectionString = "Server=tcp:group1project.database.windows.net,1433;Initial Catalog=PicturesGallery;Persist Security Info=False;User ID=manager;Password=Abcd1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        private List<ImageDetails> ExecuteQuery(string sqlQuery, object parameters = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<ImageDetails>(sqlQuery, parameters).ToList();
            }
        }

        public List<ImageDetails> GetRandomImages()
        {
            var sqlQuery = "SELECT TOP 100 * FROM Pictures ORDER BY NEWID()";
            return ExecuteQuery(sqlQuery);
        }

        public List<ImageDetails> GetImagesByKeyword(string keyword)
        {
            var sqlQuery = "SELECT * FROM Pictures WHERE title LIKE @SearchKeyword";
            return ExecuteQuery(sqlQuery, new { SearchKeyword = $"%{keyword}%" });
        }

        public List<ImageDetails> GetRelatedImages(int id)
        {
            var sqlQuery = "SELECT * FROM Pictures WHERE id != @Id AND theme = (SELECT theme FROM Pictures WHERE id = @Id)";
            return ExecuteQuery(sqlQuery, new { Id = id });
        }

        public List<ImageDetails> GetImagesByIds(string ids)
        {
            var idList = ids.Split(' ').Select(int.Parse).ToList();
            var sqlQuery = "SELECT * FROM Pictures WHERE id IN @Ids";
            return ExecuteQuery(sqlQuery, new { Ids = idList });
        }
        public List<ImageDetails> GetImagesByTag(string tag)
        {
            var sqlQuery = "SELECT * FROM Pictures WHERE tag = @Tag";
            return ExecuteQuery(sqlQuery, new { Tag = tag });
        }

        public List<ImageDetails> GetAccountsById(int id)
        {
            var sqlQuery = "SELECT * FROM Pictures WHERE id = @id";
            return ExecuteQuery(sqlQuery, new { id = id });
        }
    }
}
