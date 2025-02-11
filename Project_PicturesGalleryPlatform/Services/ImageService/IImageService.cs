using Project_PicturesGalleryPlatform.Models;

namespace Project_PicturesGalleryPlatform.Services.ImageService
{
    public interface IImageService
    {
        List<ImageDetails> GetRandomImages();
        List<ImageDetails> SearchImagesByKeyword(string keyword, int page, int pageSize);
        List<ImageDetails> GetImagesById(int id);
        List<ImageDetails> GetImagesByIds(List<int> ids, int page, int pageSize);
        List<ImageDetails> GetImagesByTag(string tag, int page, int pageSize);
    }
}
