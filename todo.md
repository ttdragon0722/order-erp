# ✅ 今日 TODO List

---

## **1️⃣ Next.js 開發**
- [ ] **Middleware 管理 `manager` 路由導向**
  - 在 `middleware.ts` 檔案內處理登入檢查邏輯  
  - 未登入導向 `/manager/login`，已登入導向 `/manager/dashboard`  

- [ ] **路由與入口管理**
  - 建立 `routeHandler.ts` 處理所有與路由、Middleware 溝通的邏輯  
  - 建議放置於 `lib/router/routeHandler.ts`  

- [ ] **Next.js Modal 特殊路由（Intercepting Routes）**
  - 使用 Next.js `Intercepting Routes` 建立 Modal，避免頁面跳轉  
  - **結構範例**：
    ```
    /app
    ├── product
    │   ├── page.tsx  (主要的產品頁)
    │   ├── @modal
    │   │   ├── product-modal.tsx  (Modal 組件)
    ```
  - `useRouter.push('/product/123')` 會打開 Modal 而不是新頁面  

- [ ] **頁面切換淡出動畫**
  - 使用 `framer-motion` 或 `GSAP` 增加過場動畫  
  - **示例**：
    ```tsx
    import { motion } from "framer-motion";

    const PageTransition = ({ children }) => (
      <motion.div initial={{ opacity: 0 }} animate={{ opacity: 1 }} exit={{ opacity: 0 }} transition={{ duration: 0.5 }}>
        {children}
      </motion.div>
    );

    export default PageTransition;
    ```

- [ ] **使用 `use server` 獲取後端資料**
  - `use server` 主要用於 **Server Actions**，但不適用 `GET` 請求  
  - 在 `server/actions.ts` 內建立 `fetchProducts`：
    ```tsx
    "use server";

    import { cookies } from "next/headers";

    export async function fetchProducts() {
      const res = await fetch("https://api.example.com/products", {
        headers: { Authorization: `Bearer ${cookies().get("authToken")}` }
      });
      return res.json();
    }
    ```

---

## **2️⃣ ASP.NET 開發**
- [ ] **撰寫 `LoginController` 登入 API**
  - 目標：實作 ASP.NET Core `api/login`，並回傳 Token  
  - **範例**：
    ```csharp
    [ApiController]
    [Route("api/login")]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (request.Username == "admin" && request.Password == "123456")
            {
                return Ok(new { message = "Login successful", token = "your_jwt_token_here" });
            }

            return Unauthorized(new { message = "Invalid username or password" });
        }
    }
    ```

- [ ] **MySQL 資料庫存取**
  - 建立 `DatabaseService.cs` 來管理資料存取  
  - **範例**：
    ```csharp
    using MySql.Data.MySqlClient;
    using System.Data;

    public class DatabaseService
    {
        private readonly string _connectionString = "Server=localhost;Database=erp;User=root;Password=1234;";

        public async Task<DataTable> GetProducts()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new MySqlCommand("SELECT * FROM Products", connection);
                var reader = await command.ExecuteReaderAsync();

                var table = new DataTable();
                table.Load(reader);
                return table;
            }
        }
    }
    ```

---

## **3️⃣ 效能與快取**
- [ ] **實作快取機制**
  - MemoryCache（適合小型快取）
  - Redis（適合大型快取）

- [ ] **使用 MemoryCache 快取**
  ```csharp
  using Microsoft.Extensions.Caching.Memory;

  public class ProductService
  {
      private readonly IMemoryCache _cache;

      public ProductService(IMemoryCache cache)
      {
          _cache = cache;
      }

      public List<Product> GetProducts()
      {
          return _cache.GetOrCreate("products_cache", entry =>
          {
              entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
              return FetchProductsFromDB();
          });
      }
  }
