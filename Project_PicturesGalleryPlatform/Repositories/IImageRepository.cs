using Project_PicturesGalleryPlatform.Models;

namespace Project_PicturesGalleryPlatform.Repositories
{
    public interface IImageRepository
    {
        List<ImageDetails> GetRandomImages();
        List<ImageDetails> GetImagesByKeyword(string keyword);
        List<ImageDetails> GetRelatedImages(int id);
        List<ImageDetails> GetImagesByIds(string ids);
        List<ImageDetails> GetImagesByTag(string tag);
        List<ImageDetails> GetAccountsById(int id);
    }
}
