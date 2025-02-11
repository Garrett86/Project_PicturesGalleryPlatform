using Project_PicturesGalleryPlatform.Models;

namespace Project_PicturesGalleryPlatform.Services.MyFavoritesService
{
    public interface IMyFavoritesService
    {
        void AddFavorite(String userAccount, int pictureId);
        void RemoveFavorite(String userAccount, int pictureId);
        List<ImageDetails> UpdateFavoritedStatusForImages(List<ImageDetails> images, string userAccount);
        List<ImageDetails> GetImagesByUserAccount(String userAccount, int page, int pagesize);
    }
}
