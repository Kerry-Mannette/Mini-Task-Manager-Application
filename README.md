# Mini Mini Task Manager Application

A Mini Task Manager is a lightweight web application built with ASP.NET MVC on .NET 10 that lets users create, manage, and track tasks. This repository demonstrates a minimal, production-oriented sample with database integration (EF Core + PostgreSQL), an example CI/CD deployment to Azure, and a responsive CRUD UI.

## Overview

The project implements a simple task management MVP and includes examples for deploying the web app to Azure App Service and using Azure Database for PostgreSQL.

## Getting started

### Prerequisites

- .NET SDK 10.0 (`dotnet`)
- Visual Studio 2022/2023 or Visual Studio Code (C# extension)
- PostgreSQL (local, Docker, or Azure Database for PostgreSQL)
- (Optional) EF Core CLI: `dotnet tool install --global dotnet-ef`
- Git
- (Optional) Azure CLI (`az`) and an Azure subscription

### Quick start (development)

```powershell
git clone https://github.com/Kerry-Mannette/Mini-Task-Manager-Application.git
cd Mini-Task-Manager-Application
dotnet restore
# Configure your connection string in appsettings.Development.json or user-secrets
dotnet ef database update
dotnet run
```

Use `dotnet watch run` for iterative development.

## Configuration

- Use `appsettings.Development.json`, environment variables, or `dotnet user-secrets` to supply a `ConnectionStrings:DefaultConnection` value when developing locally.
- In Azure App Service, put the production connection string into **Configuration > Application settings** (do not commit secrets to the repo).

## Features (MVP)

- Dashboard with a list of tasks
- Add / Edit / Delete tasks and mark complete
- Each task has Title, Description, Due Date, and Status
- Tasks stored with EF Core in a relational database (Postgres in production, SQLite/in-memory in development)

## Database & Migrations

This project uses Entity Framework Core. Apply migrations with:

```powershell
dotnet ef database update
```

When running in Development without a connection string, the app will use a local SQLite file database (see `appsettings.Development.json`).

## Deployment

- Host on Azure App Service and use Azure Database for PostgreSQL for production.
- Use GitHub Actions to automate build and deployment on push to `main`.

## Branching model

- `main` — production-ready
- `staging` — integration testing
- `feature` — active development

## Contributing

Create feature branches from `feature`, open PRs into `staging`, and merge to `main` for releases.

## Notable files

- `Program.cs` — app startup and EF Core configuration
- `Data/ApplicationDbContext.cs` — EF Core DbContext and model configuration
- `Models/TaskItem.cs` — Task entity

---

If you want, I can also add a `.gitignore` for `bin/`, `obj/`, and `*.db` files and commit it.# Mini Task Manager Application

A Mini Task Manager is a lightweight web application built with ASP.NET MVC on .NET 10 that allows users to create, manage, and track tasks. It’s designed as a demo project to showcase CI/CD deployment to Azure, database integration using PostgreSQL, and standard CRUD functionality implemented with Entity Framework Core.

## Overview

This repository demonstrates a minimal, production-oriented sample web service with a separate web application and PostgreSQL database (targeting Azure Database for PostgreSQL). The focus is the MVP feature set (task CRUD, responsive UI) plus deployment examples (Azure App Service + GitHub Actions).

## Getting Started

Prerequisites:
- .NET SDK 10.0 (`dotnet` command)
- Visual Studio 2022/2023 or Visual Studio Code with the C# extension
- PostgreSQL for development (Docker, local install, or Azure Database for PostgreSQL)
- Npgsql EF Core provider and Entity Framework Core CLI (optional):
	- `dotnet tool install --global dotnet-ef`
	- `dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL`
- Git
- Azure CLI (`az`) and an Azure account (for deployment)
- Optional: Node.js and npm (if you use front-end build tooling)

Quick start (development):

```powershell
git clone https://github.com/Kerry-Mannette/Mini-Task-Manager-Application.git
cd Mini-Task-Manager-Application
dotnet restore
# Configure your PostgreSQL connection (example env var name: DATABASE_URL or ConnectionStrings__Default)
# Example connection string (Npgsql format):
# "Host=localhost;Port=5432;Database=minitasks;Username=postgres;Password=yourpassword"
# Apply EF Core migrations (if available)
dotnet ef database update
dotnet run
```

For iterative development you can use `dotnet watch run` to auto-reload on file changes.

Configuration notes:
- Use `appsettings.Development.json` or environment variables to set the PostgreSQL connection string when running locally.
- In Azure App Service, store the connection string in **Configuration > Application settings** (use the same key name your app reads, e.g. `ConnectionStrings__Default`).

Deployment:
- Host the web app on Azure App Service and use Azure Database for PostgreSQL (Flexible Server or Single Server).
- CI/CD: a GitHub Actions workflow can build the app and deploy to Azure App Service on push to `main`. Configure the connection string in App Service settings; do not commit secrets to the repo.

Authentication (optional):
- You can add ASP.NET Core Identity for username/password sign-in. For the MVP it's optional; the project can start as an open CRUD app and Identity can be integrated later.

## Core Requirements (MVP)

User Interface

- Simple dashboard with a list of tasks
- Buttons/links for Add Task, Edit Task, Delete Task, Mark Complete
- Responsive layout (desktop + mobile)

Task Features

- Each task has: Title, Description, Due Date, Status (Pending / Completed)
- Full CRUD operations (Create, Read, Update, Delete)

Database Integration

- Store tasks in PostgreSQL (Azure Database for PostgreSQL recommended)
- Use Entity Framework Core with the Npgsql provider for ORM
- Keep connection strings in Azure App Service Configuration (or environment variables locally)

Authentication (Optional for MVP)

- Basic login using ASP.NET Core Identity (username/password), or skip for initial MVP

Deployment & CI/CD

- Host on Azure App Service
- CI/CD via GitHub Actions with an automated deploy on push to `main`

## Branching model

- `main` — production-ready code
- `staging` — pre-production integration branch
- `feature` — development and feature work

## Contributing

Create feature branches from `feature`, open pull requests into `staging`, then merge into `main` for releases.


