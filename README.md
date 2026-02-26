# Thessian Blog Showcase

A modernized ASP.NET Core MVC blog platform application.

### Product + UX
- Cinematic hero experience and visual storytelling on the landing page.
- Responsive card-based content feed optimized for desktop and mobile.
- Lightweight motion and counter animations for a premium feel without heavy dependencies.

### Engineering
- Layered architecture (Controllers + Repository + File Manager + EF Core).
- Identity-based authorization and role-aware navigation paths.
- Comment system with main and nested discussion support.
- Clean separation of concerns for extensibility and maintainability.

---

## Prerequisites

- .NET SDK **8.0.x** (project targets `net6.0`).
- SQL Server LocalDB (Windows) or SQL Server instance configured in `appsettings.json`.

Verify your SDK install:

```bash
dotnet --info
```

### EF Core CLI (required for migrations)

This project uses Entity Framework Core migrations. Install the EF CLI tool before running database update.

### Recommended (repo-local tool)
```bash
dotnet new tool-manifest
dotnet tool install dotnet-ef --version 6.*
```

Verify:
```bash
dotnet ef --version
```

## Quick start

1. Restore dependencies:
   ```bash
   dotnet restore
   ```
2. Apply migrations and create database:
   ```bash
   dotnet ef database update
   ```
3. Run the app:
   ```bash
   dotnet run
   ```
4. Open the local URL shown in the terminal.

---

## Demo credentials

- **Username**: `admin`
- **Password**: `Password1`

---

## Suggested walkthrough

1. **Landing page**: visual hierarchy and KPI snapshot.
2. **Post detail**: content experience + threaded comment workflow.
3. **Authentication and roles**: sign in as admin versus a registered user to observe permission boundaries.
4. **Admin panel**: posting flow and operational controls.
