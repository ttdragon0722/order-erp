import pandas as pd

def generate_sample_excel(output_file: str):
    # 範本資料
    data = [
        {"Name": "sample 1", "Enable": True, "Stock": False, "StockAmount": None},
        {"Name": "sample 2", "Enable": True, "Stock": True, "StockAmount": 50}
    ]
    
    # 將資料轉換為 DataFrame
    df = pd.DataFrame(data)
    
    # 儲存為 Excel 檔案
    df.to_excel(output_file, index=False)

    print(f"Sample Excel file generated: {output_file}")

# 使用範例
generate_sample_excel("materials.xlsx")
