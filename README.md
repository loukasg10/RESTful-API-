# MyApiProject
This is a simple ASP.NET Core Web API for managing employees and departments.

## Features
1.  CRUD operations for Employees and Departments

2. Filtering employees by salary range, department, etc.

3. Special endpoints like highest salary and average salary by department


## Requirements
1. .NET 5 SDK
2. SQL Server (or LocalDB)
3. Entity Framework Core

## Setup Instructions
1. git clone https://github.com/loukasg10/RESTful-API-.git

2. Configure your database connection in appsettings.json:
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=MyApiDb1;Trusted_Connection=True;"
}

3. Run EF Core migrations to create the database and tables: dotnet ef database update

4. Run the API: dotnet run



