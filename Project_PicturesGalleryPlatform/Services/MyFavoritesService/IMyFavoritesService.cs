using Project_PicturesGalleryPlatform.Models;

namespace Project_PicturesGalleryPlatform.Services.MyFavoritesService
{
    public interface IMyFavoritesService
    {
        public void AddFavorite(String userAccount, int pictureId);
        public void RemoveFavorite(String userAccount, int pictureId);
        public List<ImageDetails> UpdateFavoritedStatusForImages(List<ImageDetails> images, string userAccount);
        public List<ImageDetails> GetImagesByUserAccount(String userAccount, int page, int pagesize);
    }
}
