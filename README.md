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

## Demo credentials

- **Username**: `admin`
- **Password**: `Password1`
