using System.Diagnostics;

namespace Project_PicturesGalleryPlatform.Services.ImageAnalysisService.PythonImageAnalysisExecutor
{
    public class PythonImageAnalysisExecutor : IPythonImageAnalysisExecutor
    {
        private List<int> ExecutePythonScript(String pythonPath, String args)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = pythonPath,
                Arguments = args,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process proc = Process.Start(startInfo))
            {
                using (System.IO.StreamReader reader = proc.StandardOutput)
                {
                    String result = reader.ReadToEnd().Split("\r")[0];
                    return result.Split(' ').Select(int.Parse).ToList();
                }
            }
        }
        public List<int> FindSimilarImageIds(IFormFile file)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", file.FileName);
            return ExecutePythonScript("pythonExePath", filePath);    // 待完成
        }

        //public List<int> FindSimilarTextIds(String query)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
