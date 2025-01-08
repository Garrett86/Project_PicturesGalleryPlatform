using System.Drawing;
namespace Project_PicturesGalleryPlatform.Models.AIPicturesModels
{
    /// <summary>
    /// 此類用於將AIPicData轉換為ImageDetails以及獲取圖片尺寸
    /// </summary>
    public class DataTransformer
    {
        public AIPicData OriData { get; set; }
        int height;
        int width;

        public DataTransformer(AIPicData oriData)
        {
            this.OriData = oriData;
        }

        /// <summary>
        /// 讀取圖片url 取得圖片的尺寸
        /// </summary>
        public async Task _getSize()
        {
            using (HttpClient client = new())
            {
                try
                {
                    byte[] imageBytes = await client.GetByteArrayAsync(this.OriData.share_url);
                    using(MemoryStream ms = new(imageBytes))
                    {
                        Image image = Image.FromStream(ms);
                        this.height = image.Height;
                        this.width = image.Width;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// 將AIPicData轉換為ImageDetails
        /// </summary>
        /// <param name="keyword">目前生成圖片的關鍵字</param>
        /// <returns>imageDetail類型資料</returns>
        public ImageDetails Transform(string keyword)
        {
            _getSize().Wait();
            // 轉換類型
            ImageDetails NewData = new ImageDetails
            {
                id = 0,
                tag = keyword,
                title = this.OriData.id,
                url = this.OriData.share_url,
                height = this.height,
                width = this.width
            };
            return NewData;
        }
    }
}
