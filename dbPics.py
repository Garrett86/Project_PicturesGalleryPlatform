import pyodbc

def dbPics (title, width, height, tag):

    connectionString = (
       "Driver={ODBC Driver 17 for SQL Server};"
       "Server=tcp:group1project.database.windows.net,1433;"
       "Database=PicturesGallery;"
       "Uid=manager;"
       "Pwd=Abcd1234;"
       "Encrypt=yes;"
       "TrustServerCertificate=no;"
       "Connection Timeout=30;")
       
    conn = pyodbc.connect(connectionString)     
    cursor = conn.cursor()
    
    # 查詢目前最大id，初始化 id：若表是空的，沒有任何id，則傳回NULL，將NULL替換為0
    SQL_QUERY = "SELECT ISNULL(MAX(id), 0) FROM Pictures;"
    cursor.execute(SQL_QUERY)
    max_id = cursor.fetchone()[0]
    new_id = max_id + 1  # 設定下一筆資料的id
    
    SQL_INSERT = f"insert into Pictures(id,title,width,height,tag) values({new_id},N'{title}',{width},{height},N'{tag}');"
    cursor.execute(SQL_INSERT)
          
    cursor.commit()        
    conn.close()   
        