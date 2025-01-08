namespace Project_PicturesGalleryPlatform.Services.ImageAnalysisService.PythonImageAnalysisExecutor
{
    public interface IPythonImageAnalysisExecutor
    {
        public List<int> FindSimilarImageIds(IFormFile file);
        //public List<int> FindSimilarTextIds(String text);
    }
}
