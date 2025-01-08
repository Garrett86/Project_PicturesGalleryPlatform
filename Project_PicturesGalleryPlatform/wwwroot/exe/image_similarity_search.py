import os
import numpy as np
from tensorflow.keras.applications.vgg16 import VGG16, preprocess_input
from tensorflow.keras.preprocessing import image
from sklearn.metrics.pairwise import cosine_similarity
from concurrent.futures import ThreadPoolExecutor
from flask import Flask, request, jsonify
from io import BytesIO
from PIL import Image
import sys
from flask_cors import CORS

app = Flask(__name__)
CORS(app)

# 獲取執行目錄及父目錄
exe_dir = os.path.dirname(sys.executable)
parent_dir = os.path.dirname(exe_dir)

# 載入VGG16模型 (不包含頂層)
model = VGG16(weights='imagenet', include_top=False, input_shape=(224, 224, 3))

# 設置特徵緩存路徑
feature_cache_folder = os.path.join(parent_dir, 'features_cache')
if not os.path.exists(feature_cache_folder):
    os.makedirs(feature_cache_folder)

# 提取圖片特徵並L2正規化
def extract_features(img_path):
    cache_path = os.path.join(feature_cache_folder, f"{os.path.basename(img_path)}.npy")
    
    # 若特徵已經存在，則載入
    if os.path.exists(cache_path):
        return np.load(cache_path)
    
    # 若沒有特徵則提取新圖片特徵
    img = image.load_img(img_path, target_size=(224, 224))
    img_array = np.expand_dims(image.img_to_array(img), axis=0)
    img_array = preprocess_input(img_array)
    
    features = model.predict(img_array).flatten()
    normalized_features = features / np.linalg.norm(features)
    
    # 儲存特徵
    np.save(cache_path, normalized_features)
    return normalized_features

# 圖片資料夾路徑
image_folder = os.path.join(parent_dir, 'downloaded_images')
image_paths = [os.path.join(image_folder, img) for img in os.listdir(image_folder) 
               if img.lower().endswith(('.png', '.jpg', '.jpeg', 'webp'))]

# 多執行緒加速特徵提取
with ThreadPoolExecutor(max_workers=16) as executor:
    features_list = list(executor.map(extract_features, image_paths))

# 上傳圖片並查詢相似圖片
@app.route('/upload', methods=['POST'])
def upload_image():
    # 檢查是否有文件
    if 'file' not in request.files:
        return jsonify({'error': 'No file part'}), 400
    
    file = request.files['file']
    
    # 檢查文件名稱
    if file.filename == '':
        return jsonify({'error': 'No selected file'}), 400

    # 讀取並處理上傳圖片
    img = Image.open(BytesIO(file.read())).resize((224, 224))
    img_array = np.expand_dims(image.img_to_array(img), axis=0)
    img_array = preprocess_input(img_array)
    
    # 提取圖片特徵
    features = model.predict(img_array).flatten()
    img_features = features / np.linalg.norm(features)

    # 計算相似度
    similarities = cosine_similarity([img_features], features_list)[0]
    top_indices = np.argsort(similarities)[::-1]

    # 設置相似度閾值
    similarity_threshold = eval(sys.argv[1]) if len(sys.argv) > 1 else 0.15
    
    # 篩選相似圖片
    filtered_image_names = [
        os.path.splitext(os.path.basename(image_paths[idx]))[0] 
        for idx in top_indices if similarities[idx] > similarity_threshold
    ]

    print({'filtered_image_names': filtered_image_names})
    
    # 返回結果
    return jsonify({'filtered_image_names': filtered_image_names})

if __name__ == '__main__':
    app.run(debug=True, port=5001, use_reloader=False)
