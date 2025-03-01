# User 資料表設計與功能說明

## 1. User 資料表結構

| 欄位名稱   | 型別                   | 說明 |
|------------|------------------------|-------------------------------------------|
| `id`       | `CHAR(36)` (UUID)       | 主鍵，使用 UUID 作為唯一識別碼            |
| `userId`   | `VARCHAR(255)`          | 帳號（唯一），用於登入系統               |
| `password` | `VARCHAR(255)`          | 加密後的密碼                              |
| `salt`     | `VARCHAR(255)`          | Salt（鹽值），用來加強密碼安全性          |
| `name`     | `VARCHAR(255)`          | 使用者名稱                                |
| `role`     | `VARCHAR(50)`           | 使用者角色（Admin/User）                  |
| `createdAt`| `DATETIME`              | 帳號建立時間                              |
| `updatedAt`| `DATETIME`              | 帳號最後更新時間                          |
| `isActive` | `TINYINT(1) DEFAULT 1`  | 帳號是否啟用（0 = 停用, 1 = 啟用）         |

---

## 2. 各欄位說明

### **id (主鍵)**
- 使用 `UUID` 作為主鍵，確保唯一性。
- UUID 比數字 ID 更安全，避免預測性攻擊。

### **userId (帳號)**
- 使用者的登入帳號，需 **唯一**，不可重複。
- 可設為 Email 或其他識別名稱。

### **password (密碼)**
- 儲存 **加密後的密碼**，不存明文。
- 密碼雜湊使用 **BCrypt** 或其他安全的演算法。

### **salt (鹽值)**
- 每個使用者的密碼都有不同的 Salt，提高加密強度。
- Salt 會與密碼組合後進行加密。

### **name (使用者名稱)**
- 主要用於顯示的使用者名稱，與 `userId` 無關。
- 可允許重複。

### **role (角色)**
- 權限管理的角色欄位，例如：
  - `Admin`：管理者
  - `User`：一般使用者
  
### **createdAt (建立時間)**
- 記錄帳號創建的時間。
- 預設值為當前時間。

### **updatedAt (最後更新時間)**
- 當使用者修改資訊時，會更新這個欄位。

### **isActive (帳號狀態)**
- 是否啟用帳號。
- `1` = 啟用，`0` = 停用。
- 停用帳號的使用者無法登入。

---

## 3. User 資料表的 Migration

如果使用 **Entity Framework Core**，可以這樣建立 `User` 資料表：

```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required, MaxLength(255)]
    public string UserId { get; set; }
    
    [Required, MaxLength(255)]
    public string Password { get; set; }
    
    [Required, MaxLength(255)]
    public string Salt { get; set; }
    
    [MaxLength(255)]
    public string Name { get; set; }
    
    [Required, MaxLength(50)]
    public string Role { get; set; } = "User";
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsActive { get; set; } = true;
}
```

然後在 `DbContext` 中加入：

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<User>()
        .HasIndex(u => u.UserId)
        .IsUnique();
}
```

---

## 4. 密碼加密與驗證

建議使用 `BCrypt.Net-Next` 來進行密碼加密與驗證。

### **安裝 BCrypt**
```sh
dotnet add package BCrypt.Net-Next
```

### **密碼加密 & 驗證 Helper**
```csharp
using BCrypt.Net;

public static class PasswordHelper
{
    public static string HashPassword(string password, out string salt)
    {
        salt = BCrypt.Net.BCrypt.GenerateSalt();  // 產生隨機 Salt
        return BCrypt.Net.BCrypt.HashPassword(password + salt);  // 雜湊加密
    }

    public static bool VerifyPassword(string inputPassword, string salt, string storedHash)
    {
        return BCrypt.Net.BCrypt.Verify(inputPassword + salt, storedHash);
    }
}
```

---

## 5. 登入與註冊 API

### **註冊 API**
```csharp
[HttpPost("register")]
public async Task<IActionResult> Register([FromBody] RegisterRequest request)
{
    if (_context.Users.Any(u => u.UserId == request.UserId))
        return BadRequest("帳號已存在");
    
    string salt;
    string hashedPassword = PasswordHelper.HashPassword(request.Password, out salt);

    var user = new User
    {
        UserId = request.UserId,
        Password = hashedPassword,
        Salt = salt,
        Name = request.Name,
        Role = "User"
    };

    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    return Ok("註冊成功");
}
```

### **登入 API**
```csharp
[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginRequest request)
{
    var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId);
    if (user == null)
        return Unauthorized("帳號或密碼錯誤");

    if (!PasswordHelper.VerifyPassword(request.Password, user.Salt, user.Password))
        return Unauthorized("帳號或密碼錯誤");

    return Ok("登入成功");
}
```

---

## 6. 結論
- `User` 資料表包含帳號、密碼、角色等資訊。
- 密碼應該使用 **BCrypt** 加密，並存儲 **Salt**。
- 登入與註冊 API 需確保密碼驗證機制正確。
- 可以使用 **Entity Framework Core** 來建立 `User` 資料表與 Migration。

這樣就完成了一個基本的 **帳號管理系統**！ 🚀

