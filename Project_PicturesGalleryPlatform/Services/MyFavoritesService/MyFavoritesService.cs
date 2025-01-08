using Project_PicturesGalleryPlatform.Models;
using Project_PicturesGalleryPlatform.Repositories.MyFavoritesRepository;
using Project_PicturesGalleryPlatform.Services.ImageService;

namespace Project_PicturesGalleryPlatform.Services.MyFavoritesService
{
    public class MyFavoritesService : IMyFavoritesService
    {
        private readonly IImageService _imageService;
        private readonly IMyFavoritesRepository _myFavoritesRepository;

        public MyFavoritesService(IImageService imageService, IMyFavoritesRepository myFavoritesRepository)
        {
            _imageService = imageService;
            _myFavoritesRepository = myFavoritesRepository;
        }

        // 添加圖片到收藏
        public void AddFavorite(string userAccount, int pictureId) =>
            _myFavoritesRepository.AddFavorite(userAccount, pictureId);

        // 從收藏中移除圖片
        public void RemoveFavorite(string userAccount, int pictureId) =>
            _myFavoritesRepository.RemoveFavorite(userAccount, pictureId);

        // 更新圖片收藏狀態
        public List<ImageDetails> UpdateFavoritedStatusForImages(List<ImageDetails> images, string userAccount)
        {
            var favoritePictureIds = new HashSet<int>(_myFavoritesRepository.GetUserFavoritePictureIds(userAccount));
            foreach (var image in images)
                image.isFavorited = favoritePictureIds.Contains(image.id);
            return images;
        }

        // 根據用戶帳號獲取收藏圖片
       public List<ImageDetails> GetImagesByUserAccount(string userAccount, int page, int pageSize)
        {
            var ids = _myFavoritesRepository.GetUserFavoritePictureIds(userAccount);
            return _imageService.GetImagesByIds(ids, page, pageSize);
        }
    }
}
