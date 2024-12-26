import json
from check_images import load_finishDn_images
from image_crawler import imageSearch
from dbPics import dbPics


finishDn_file = "finishDn_images.json"
folder_path = "./downloaded_images"
themes = ["dessert"]
result = []

# 讀取已下載的圖片清單
finishDn_images = load_finishDn_images(finishDn_file)

for theme in themes:
    imageSearch(theme, 3, finishDn_images, finishDn_file, folder_path, dbPics, result)

# 儲存結果
with open("imageData.json", "w", encoding="utf-8-sig") as file:
    json.dump(result, file, ensure_ascii=False, indent=4)

