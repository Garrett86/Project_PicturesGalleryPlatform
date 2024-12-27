using Project_PicturesGalleryPlatform.Models;
using Project_PicturesGalleryPlatform.Repositories;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace Project_PicturesGalleryPlatform.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private static List<ImageDetails> _currentImageResults;

        public ImageService(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public List<ImageDetails> GetRandomImages()
        {
            _currentImageResults = _imageRepository.GetRandomImages();
            return _currentImageResults;
        }

        public List<ImageDetails> GetImagesByKeyword(string keyword)
        {
            _currentImageResults = _imageRepository.GetImagesByKeyword(keyword);
            return _currentImageResults;
        }

        public List<ImageDetails> GetImagesByPage(int page, int pageSize)
        {
            return _currentImageResults?.Skip(page * pageSize).Take(pageSize).ToList() ?? new List<ImageDetails>();
        }

        public List<ImageDetails> GetAccountsById(int id)
        {
            _currentImageResults = _imageRepository.GetAccountsById(id);
            return _currentImageResults;
        }

        public List<ImageDetails> GetAccountsByTag(string tag)
        {
            _currentImageResults = _imageRepository.GetImagesByTag(tag);
            return _currentImageResults;
        }
    }
}
