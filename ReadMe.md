# 🚀 StreamTable

[![Framework](https://img.shields.io/badge/ASP.NET_Core-10.0-6222C0?style=flat-square&logo=.net)](https://dotnet.microsoft.com/)
[![Reactive](https://img.shields.io/badge/RxJS-7.8-D81B60?style=flat-square&logo=reactivex)](https://rxjs.dev/)
[![Database](https://img.shields.io/badge/LocalDB-MSSQL-blue?style=flat-square&logo=microsoft-sql-server)](https://www.microsoft.com/en-us/sql-server/)

# StreamTable
> StreamTable is a high-performance data grid solution for ASP.NET Core that solves the "Large Dataset" problem. Instead of waiting for a 100MB JSON payload to download and parse, StreamTable uses HTTP Response Streaming and RxJS to render data in the browser the millisecond it leaves the SQL Server.

# 💡 Why StreamTable?
Stop waiting for massive JSON payloads. **StreamTable** uses Chunked Transfer to stream rows into the UI instantly as soon as sql starts returning rows, bypassing the 'all-or-nothing' limitation of traditional AJAX. By removing ORDER BY and OFFSET/FETCH (paging) from your SQL query and web server, you are essentially "taking the handcuffs off" the database engine and achive same thing with **DataTable.js**

| Feature | Description |
| :--- | :--- |
| **RxJS Throttle** | Prevents UI freezing by batching updates every 100ms. |
| **Memory Efficient** | Processes data in chunks rather than loading a massive JSON array. |
| **Automated Seeding** | Includes a SQL script to populate LocalDB instantly. |

# 🎯 Overview
> Traditional data grids often struggle with large datasets due to the need to load and parse the entire dataset before rendering. StreamTable addresses this by leveraging ASP.NET Core's ability to stream data directly from the database to the client, allowing for near-instantaneous rendering of rows as they arrive.

# 🚀 Key Features
> EF Core SQL Seeding: Custom infrastructure to seed large datasets (200k+ rows) directly from .sql files during database initialization.
Sequential Data Streaming: Uses IAsyncEnumerable to stream rows from EF Core without buffering the entire list in server memory.
RxJS Observable Integration: Frontend treats the HTTP stream as an Observable, allowing for clean data handling and "push-based" UI updates.
Smart Chunk Parsing: Includes a custom buffer logic to handle partial JSON fragments caused by network packet splitting.
Zero-Lock UI: Processes data in batches to ensure the grid remains responsive (60fps) even while thousands of rows are being inserted.
datatablejs Integration: Built on top of datatables.js which allow sorting, filtering, and pagination as soon as data is available.

# 🛠 Tech Stack
> Backend: ASP.NET Core 10, Entity Framework Core (SQL Server)
  Frontend: JavaScript (ES6+), RxJS, DataTables.js
  Database: LocalDB / MS SQL Server

# 📂 Project Structure

    StreamTable/
    ├── Data/
    │   ├── ApplicationDbContext.cs   # EF Core Context
    │   ├── SampleCustomers.sql       # Raw SQL seed data
    │   └── DbInitializer.cs          # Async DB creation & seeding logic
    ├── Models/
    │   └── Customer.cs               # Data Entity
    ├── Controllers/
    │   └── CustomerController.cs     # IAsyncEnumerable Streaming API
    ├── wwwroot/
    │   └── js/
    │       └── StreamService.js      # RxJS Stream consumer logic
    │       └── customerTable.js      # Load customer data into datatable
    └── Program.cs                    # App startup & Middleware


# ⚙️ Getting Started

## 1. Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) 
- SQL Server LocalDB

## 2. Database Setup
> The project is configured to automatically create the database and seed it from the SampleCustomers.sql file on the first run.
- Verify Installation
    Ensure LocalDB is installed on your machine. Open PowerShell or CMD and type:
```bash
   sqllocaldb v
   ```
   If you see "Microsoft SQL Server 2022...", you are ready. If not, install it via the Visual Studio Installer.
- Start the Instance
    If your connection string uses (localdb)\mssqllocaldb, ensure that specific instance is created and running:
```bash
sqllocaldb create mssqllocaldb
sqllocaldb start mssqllocaldb
 ```
 - Connection String
 ```bash
 "ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=StreamTableDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
 ```

## 3. Running the App
```bash
   dotnet watch run --project streamtable
   ```
