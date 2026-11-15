# RoleBasedEmployeeApp

An employee management system built with **ASP.NET Core MVC (.NET 10)**, **ADO.NET** and **SQL Server stored procedures**. It has cookie-based login, database-driven menus, role-based permissions and an admin panel.

## Features

- Login and logout with cookie authentication and BCrypt password hashing
- Three roles: **Admin**, **HR** and **Viewer**
- Left-hand sidebar loaded from the database, showing only the menus the logged-in role may view
- Per-menu permissions (View, Add, Edit, Delete), checked in the controllers and used to hide buttons in the views
- Access Denied page for anything a role is not allowed to do
- **Employees** CRUD with a department dropdown
- **Admin panel**
  - Users: list users and add a user (password stored as a BCrypt hash)
  - Roles & Permissions: a checkbox grid to change what each role can do
  - Settings: edit key/value settings such as the site name

## Tech stack

| Layer | Technology |
|---|---|
| Web | ASP.NET Core MVC, Razor views, Bootstrap |
| Language | C# (.NET 10) |
| Data access | ADO.NET (`Microsoft.Data.SqlClient`), stored procedures |
| Database | SQL Server Express |
| Security | Cookie authentication, BCrypt.Net |

## Requirements

- Visual Studio 2026 (or 2022) with the **ASP.NET and web development** workload
- .NET 10 SDK
- SQL Server Express (`localhost\SQLEXPRESS`) and SQL Server Management Studio (SSMS)

## Setup

1. **Clone the repository**
   ```
   git clone https://github.com/YashBuddhivant/RoleBasedEmployeeApp.git
   ```
2. **Create the database.** Open `database.sql` in SSMS and run the whole script. It creates the `RoleAdminDb` database, the tables, the seed data and all stored procedures. Running it again drops and recreates the database.
3. **Check the connection string** in `ConnectionString.cs`. The default is:
   ```
   Server=localhost\SQLEXPRESS; Database=RoleAdminDb; Trusted_Connection=True; TrustServerCertificate=True;
   ```
   Change the server name if your SQL Server instance is named differently.
4. **Run the project.** Open the solution in Visual Studio and press **F5**.

## Test logins

| Username | Password | Role | What they can do |
|---|---|---|---|
| `admin` | `Admin@123` | Admin | Everything, including the admin panel |
| `hr1` | `Hr@12345` | HR | View, add and edit employees |
| `viewer1` | `Viewer@123` | Viewer | View employees only |

These are test accounts for local use only. Change the passwords before any real deployment.

## How permissions work

1. At login, the user's role name and `RoleId` are stored as claims in the cookie.
2. Each menu in the `Menus` table has a URL (for example `/Employee`). The `RoleMenuPermissions` table stores View, Add, Edit and Delete flags for each role and menu.
3. In each controller, the `spGetPermission` procedure looks up the flags by role and menu URL.
4. The controller redirects to Access Denied if the action is not allowed, and the view hides buttons the user can't use.

Hiding a button is only cosmetic. The check in the controller is what protects the action.

## Project structure

```
RoleBasedEmployeeApp/
├── Controllers/      Account, Home, Employee, User, Role, Setting, PermissionControllerBase
├── Models/           Employee, User, Menu, Permission, view models
├── Views/            Razor views (one folder per controller, plus Shared)
├── *DataAccessLayer.cs   ADO.NET code that calls the stored procedures
├── ConnectionString.cs
├── Program.cs        Cookie authentication and routing
└── database.sql      Tables, seed data and stored procedures
```

## Database tables

`Roles`, `Users`, `Menus`, `RoleMenuPermissions`, `Settings`, `Departments`, `Employees`

## Possible improvements

- Departments CRUD
- Dashboard page
- Edit, deactivate and reset password for users
- Move the connection string to `appsettings.json` or User Secrets

## Author

Yash Buddhivant, BSc Computer Science. Built as part of a .NET internship assignment at Winners IT Solutions.
