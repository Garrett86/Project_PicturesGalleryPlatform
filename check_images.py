import os
import json

# 讀取已下載的圖片清單
def load_finishDn_images(file_path):   
    if os.path.exists(file_path):
        with open(file_path, 'r', encoding='utf-8') as f:
            return set(tuple(img) for img in json.load(f))
    return set()

# 儲存已下載的圖片清單
def save_finishDn_images(finishDn_images, file_path):    
    with open(file_path, 'w', encoding='utf-8') as f:
        json.dump(list(finishDn_images), f)
