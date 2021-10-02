以下以`PostgreSQL`為例

- 套件安裝

先裝安裝`Microsoft.EntityFrameworkCore.Design` 和 `Npgsql.EntityFrameworkCore.PostgreSQL`

```bash
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
```

- 程式碼

在`appsetting.json` 中加上連線DB 的資訊

```json
"connectionStrings": {
    "EmployeeConnectionString": "Host=localhost;Port=5432;Database=test1;Username=postgres;Password=guest"
}
```

並且在程式碼中使用

```csharp
services.AddDbContextPool<EmployeeContext>(options => options.UseNpgsql(Configuration.GetConnectionString("EmployeeConnectionString")));
```

程式碼細節先跳過

- 指令執行

要建造DB 和 Table 時，在目標資料夾中的command 中輸入

```bash
dotnet ef migrations add [migration name] --context [context name] # 建立 migrations
dotnet ef migrations add InitialCreate --context EmployeeContext

dotnet ef migrations remove --context [context name] # 移除 migrations
dotnet ef migrations remove --context EmployeeContext

dotnet ef database update --context [context name] # 建立 DB 和 table
dotnet ef database update --context EmployeeContext
```

ref : [https://ithelp.ithome.com.tw/articles/10235984](https://ithelp.ithome.com.tw/articles/10235984)

ref : [https://www.youtube.com/watch?v=r4LlIhyQ9GY&ab_channel=SameerSaini](https://www.youtube.com/watch?v=r4LlIhyQ9GY&ab_channel=SameerSaini)
