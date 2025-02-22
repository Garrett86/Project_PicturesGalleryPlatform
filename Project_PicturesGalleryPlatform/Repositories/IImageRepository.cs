﻿using Project_PicturesGalleryPlatform.Models;

namespace Project_PicturesGalleryPlatform.Repositories
{
    public interface IImageRepository
    {
        List<ImageDetails> GetRandomImages();
        List<ImageDetails> GetImagesByKeyword(string keyword);
        List<ImageDetails> GetRelatedImages(int id);
        List<ImageDetails> GetImagesByTheme(string theme);
        List<ImageDetails> GetAccountsById(int id);
    }
}
