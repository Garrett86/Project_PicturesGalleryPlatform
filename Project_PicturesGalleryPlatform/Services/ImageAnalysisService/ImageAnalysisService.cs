using Project_PicturesGalleryPlatform.Models;
using Project_PicturesGalleryPlatform.Services.ImageAnalysisService.PythonImageAnalysisExecutor;
using Project_PicturesGalleryPlatform.Services.ImageService;

namespace Project_PicturesGalleryPlatform.Services.ImageAnalysisService
{
    public class ImageAnalysisService : IImageAnalysisService
    {
        private readonly IImageService _imageService;
        private readonly IPythonImageAnalysisExecutor _pythonImageAnalysisExecutor;
        public ImageAnalysisService(IImageService imageService, IPythonImageAnalysisExecutor pythonImageAnalysisExecutor)
        {
            _imageService = imageService;
            _pythonImageAnalysisExecutor = pythonImageAnalysisExecutor;
        }
        //public List<ImageDetails> FindSimilarImagesByImage(IFormFile file)
        //{
        //    var ids = _pythonImageAnalysisExecutor.FindSimilarImageIds(file);
        //    return _imageService.GetImagesByIds(ids);
        //}
        //public List<ImageDetails> SearchImagesByText()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
