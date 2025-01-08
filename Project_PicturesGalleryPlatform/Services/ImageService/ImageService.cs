using Azure.Core;
using Project_PicturesGalleryPlatform.Models;
using Project_PicturesGalleryPlatform.Repositories.ImageRepository;

namespace Project_PicturesGalleryPlatform.Services.ImageService
{
    // 處理圖片相關的業務邏輯
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;

        // 初始化 repository
        public ImageService(IImageRepository imageRepository) => _imageRepository = imageRepository;

        // 取得隨機圖片
        public List<ImageDetails> GetRandomImages() => _imageRepository.GetRandomImages();

        // 根據關鍵字搜尋圖片
        public List<ImageDetails> SearchImagesByKeyword(string keyword, int page, int pageSize) =>
            _imageRepository.SearchImagesByKeyword(keyword, page, pageSize);

        // 依 ID 取得圖片
        public List<ImageDetails> GetImagesById(int id) => _imageRepository.GetImagesById(id);

        // 依多個 ID 取得圖片
        public List<ImageDetails> GetImagesByIds(List<int> ids, int page, int pageSize) =>
            _imageRepository.GetImagesByIds(ids, page, pageSize);

        // 根據標籤取得圖片
        public List<ImageDetails> GetImagesByTag(string tag, int page, int pageSize) =>
            _imageRepository.GetImagesByTag(tag, page, pageSize);
    }
}
