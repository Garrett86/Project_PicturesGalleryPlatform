using Project_PicturesGalleryPlatform.Models;

namespace Project_PicturesGalleryPlatform.Repositories.MyFavoritesRepository
{
    public interface IMyFavoritesRepository
    {
        public void AddFavorite(String userAccount, int pictureId);
        public void RemoveFavorite(String userAccount, int pictureId);
        public List<int> GetUserFavoritePictureIds(String userAccount);
    }
}
