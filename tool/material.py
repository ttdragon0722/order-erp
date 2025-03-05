import pandas as pd
import uuid
import os

def generate_insert_statements(excel_file: str, output_folder: str):
    # 讀取 Excel 文件
    df = pd.read_excel(excel_file)

    # 確保存在 output 資料夾
    if not os.path.exists(output_folder):
        os.makedirs(output_folder)

    # SQL INSERT 生成語句
    insert_statements = []

    for index, row in df.iterrows():
        # 自動生成 UUID
        id_value = str(uuid.uuid4())
        name = row['Name']
        print(name)
        enable = '1' if row['Enable'] else '0'
        stock = '1' if row['Stock'] else '0'
        stock_amount = row['StockAmount'] if pd.notnull(row['StockAmount']) else 'NULL'

        # 生成 INSERT 語句
        insert_statement = f"INSERT INTO materials (Id, Name, Enable, Stock, StockAmount) VALUES ('{id_value}', '{name}', {enable}, {stock}, {stock_amount});"
        insert_statements.append(insert_statement)

    # 輸出到 output 資料夾
    output_file = f"{output_folder}/insert_statements.sql"
    with open(output_file, 'w',encoding="utf-8") as file:
        file.write("\n".join(insert_statements))

    print(f"Insert statements saved to {output_file}")

# 使用範例
generate_insert_statements("materials.xlsx", "output")
