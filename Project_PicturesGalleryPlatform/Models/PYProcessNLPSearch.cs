using Project_PicturesGalleryPlatform.Models.AIPicturesModels;
using System.Diagnostics;
using System.Text.Json;

namespace Project_PicturesGalleryPlatform.Models
{
    public class PYProcessNLPSearch
    {
        // Python程式路徑
        private readonly string pythonExePath = @".\wwwroot\exe\searchNLP_V14.exe";
        // 要傳遞給Python的參數
        public string arguments;
        // 創建一個進程啟動信息對象
        ProcessStartInfo startInfo;
        /// <summary>
        /// 建構式: 生成圖片關鍵字+一次生成數量
        /// </summary>
        /// <param name="keyword">生成圖片關鍵字</param>
        /// <param name="number">一次生成數量</param>
        public PYProcessNLPSearch(string keyword)
        {
            this.arguments = $"{keyword}";
            startInfo = new ProcessStartInfo
            {
                FileName = pythonExePath,
                Arguments = this.arguments,
                RedirectStandardOutput = true, // 重定向標準輸出
                UseShellExecute = false,
                CreateNoWindow = true
            };
        }
        /// <summary>
        /// 執行python生成圖片，整理生成的圖片數據
        /// </summary>
        /// <returns></returns>
        public List<int>? Search()
        {
            // *test，區域封閉
            //goto skip_testZone;
            // test
            try
            {
                // 啟動進程
                using (Process process = Process.Start(startInfo))
                {
                    // 讀取標準輸出
                    using (StreamReader reader = process.StandardOutput)
                    {
                        String result = reader.ReadToEnd();
                        return result.Split(' ').Select(int.Parse).ToList();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"執行Python可執行文件時出錯: {e.Message}");
                return null;
            }
        }
    }
}
