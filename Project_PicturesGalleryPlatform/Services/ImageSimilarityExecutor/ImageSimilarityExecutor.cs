using System.Diagnostics;

namespace Project_PicturesGalleryPlatform.Services.ImageSimilarityExecutor
{
    public class ImageSimilarityExecutor : IImageSimilarityExecutor
    {
        private readonly String currentDirectory = Directory.GetCurrentDirectory();  // 根目錄

        public void StartImageSimilaritySearch()
        {
            new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(currentDirectory, "wwwroot", "exe", "image_similarity_search.exe"),  // 目標路徑
                    Arguments = "",
                    RedirectStandardOutput = false,  // 無須接收輸出
                    UseShellExecute = true,  // 顯示命令提示字元
                    CreateNoWindow = true  // 不顯示命令提示字元
                }
            }.Start();
        }
    }
}
