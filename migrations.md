# 使用 Entity Framework Migrations

Migrations 讓你可以在 **不刪除現有資料的情況下** 更新資料庫結構，適用於開發過程中持續修改資料表的需求。

---

## **1️⃣ 建立 Migrations**
當你的 `DbContext`（`ERPDBContext`）和模型類別（`Product`、`Material` 等）都準備好後，執行：
```sh
dotnet ef migrations add InitialCreate
```
這會在專案中 **生成 `Migrations` 資料夾**，內含：
- `[時間戳] _InitialCreate.cs`（定義資料表結構）
- `[時間戳] _InitialCreate.Designer.cs`（EF 追蹤版本用）
- `ERPDBContextModelSnapshot.cs`（快照）

---

## **2️⃣ 更新資料庫**
將 Migrations 套用到資料庫：
```sh
dotnet ef database update
```
這會：
✅ 在資料庫中建立對應的表格  
✅ 自動執行 `OnModelCreating` 設定的關聯  

---

## **3️⃣ 新增欄位或修改資料表**
假設你現在想 **在 `Product` 表新增 `description` 欄位**：
### **➜ 修改 Model 類別**
```csharp
public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public bool Enable { get; set; }
    
    // 新增的欄位
    public string Description { get; set; }
}
```
### **➜ 產生新的 Migrations**
```sh
dotnet ef migrations add AddProductDescription
```
這會產生一個新的遷移檔案，例如 `20250301_AddProductDescription.cs`，裡面包含：
```csharp
migrationBuilder.AddColumn<string>(
    name: "Description",
    table: "product",
    nullable: true);
```
### **➜ 更新資料庫**
```sh
dotnet ef database update
```
這樣 `product` 表就會多出 `description` 欄位！

---

## **4️⃣ 回滾 Migrations**
如果發現問題，需要回到 **上一個版本**：
```sh
dotnet ef migrations remove
```
這會 **刪除最新的 Migrations，但不影響資料庫**。

如果已經更新資料庫了，但想回到之前的狀態：
```sh
dotnet ef database update [舊的 Migration 名稱]
```
例如回到 `InitialCreate`：
```sh
dotnet ef database update InitialCreate
```
這會讓資料庫回到這個版本的結構。

---

## **📌 總結**
1️⃣ **新增 Migrations**：`dotnet ef migrations add [名稱]`  
2️⃣ **更新資料庫**：`dotnet ef database update`  
3️⃣ **修改資料表**：更新 Model → 建立新 Migrations → 更新資料庫  
4️⃣ **回滾 Migrations**：  
   - `dotnet ef migrations remove`（只移除 Migrations）  
   - `dotnet ef database update [舊版本]`（回到舊的資料庫狀態）  

如果還有問題，隨時問我！💪