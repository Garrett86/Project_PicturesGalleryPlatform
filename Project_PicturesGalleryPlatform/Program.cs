using Project_PicturesGalleryPlatform.Repositories.ImageRepository;
using Project_PicturesGalleryPlatform.Repositories.MyFavoritesRepository;
using Project_PicturesGalleryPlatform.Services.ImageService;
using Project_PicturesGalleryPlatform.Services.MyFavoritesService;
using Microsoft.EntityFrameworkCore;
using Project_PicturesGalleryPlatform.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Project_PicturesGalleryPlatform.Repositories.IRatingService;
using Project_PicturesGalleryPlatform.Services.ImageSimilarityExecutor;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddDataAnnotationsLocalization();

// �����Y�ώ����   David add the following 2 lines
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IRatingService, RatingService>();

builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();

builder.Services.AddScoped<IMyFavoritesService, MyFavoritesService>();
builder.Services.AddScoped<IMyFavoritesRepository, MyFavoritesRepository>();

builder.Services.AddSingleton<IImageSimilarityExecutor, ImageSimilarityExecutor>();


builder.Services.AddControllersWithViews();
builder.Services.AddSession();// for session
builder.Services.AddSingleton<ITempDataProvider, SessionStateTempDataProvider>();// 使用session存取TempData

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();// for session
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Lifetime.ApplicationStarted.Register(() =>
{
    app.Services.GetRequiredService<IImageSimilarityExecutor>().StartImageSimilaritySearch();  // 開啟 pythonAPI
});

app.Run();
