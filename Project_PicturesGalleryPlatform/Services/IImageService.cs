using Project_PicturesGalleryPlatform.Models;

namespace Project_PicturesGalleryPlatform.Services
{
    public interface IImageService
    {
        List<ImageDetails> GetRandomImages();
        List<ImageDetails> GetImagesByKeyword(string keyword);
        List<ImageDetails> GetImagesByPage(int page, int pageSize);
        List<ImageDetails> GetAccountsById(int id);
        List<ImageDetails> GetAccountsByTag(string tag);
    }
}
