using Project_PicturesGalleryPlatform.Models;

namespace Project_PicturesGalleryPlatform.Repositories.IRatingService
{
    public interface IRatingService
    {
        public bool UpdateRating(int productId, string username, byte score);

        public bool AddpictureScore(int productId,string username, byte score);
        public bool IsPictureInpictureScore(String userAccount, int pictureId);
        public List<int> GetUserpictureScorePictureIds(String userAccount);

        public List<PictureScore> GetUserTotalScore();
    }
}
