using Project_PicturesGalleryPlatform.Models;

namespace Project_PicturesGalleryPlatform.Repositories.ImageRepository
{
    public interface IImageRepository
    {
        List<ImageDetails> GetRandomImages();
        List<ImageDetails> SearchImagesByKeyword(string keyword, int page, int pageSize);
        List<ImageDetails> GetRelatedImagesById(int id);
        List<ImageDetails> GetImagesByIds(List<int> ids, int page, int pageSize);
        List<ImageDetails> GetImagesByTag(string tag, int page, int pageSize);
        List<ImageDetails> GetImagesById(int id);
    }
}
