# How to run

## 1. Requirements

- .NET 8
- SQL Server
- Entity Framework Core CLI
```
dotnet tool install --global dotnet-ef
```

## 2. Setup database

1. Create a database in SQL Server
2. Update the `ConnectionString` in `/src/Api/appsettings.json`
3. Run the migration. Starting from the repo root:
```
cd src\Api
dotnet ef database update
```

## 3. Run the project