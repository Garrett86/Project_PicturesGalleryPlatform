namespace Project_PicturesGalleryPlatform.Models
{
    public class ImageDetails
    {
        public int id { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string tag { get; set; }
        public bool isFavorited { get; set; } = false;
    }
}
