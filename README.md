📦 Clean Architecture Web API (.NET)
📌 Overview

This project is a fully layered ASP.NET Core Web API built using Clean Architecture principles with CQRS, MediatR, Entity Framework Core, JWT authentication, and FluentValidation.

The system has been developed iteratively through multiple refactors and improvements, focusing on:

architectural consistency
separation of concerns
secure authentication and authorization
maintainable CQRS-based structure
robust domain modeling
🏗️ Architecture

The solution follows strict Clean Architecture separation:

🔹 API Layer
Controllers (REST-compliant endpoints)
Middleware (centralized exception handling)
JWT authentication & authorization setup
Swagger configuration
🔹 Application Layer
CQRS (Commands & Queries)
MediatR handlers
DTOs (Mapster-based mapping)
FluentValidation validators
Pipeline behaviors (validation pipeline)
Application service abstractions
🔹 Domain Layer
Core entities
Interfaces
Enums
Domain rules and constraints
🔹 Infrastructure Layer
EF Core DbContext
Repository implementations
Database migrations
Persistence configuration

👉 All dependencies flow inward toward the Domain layer.

⚙️ Technologies Used
ASP.NET Core Web API
Clean Architecture
CQRS (Command Query Responsibility Segregation)
MediatR
Entity Framework Core
Repository Pattern
FluentValidation
Mapster (DTO mapping)
JWT Authentication
Role-Based Access Control (RBAC)
SQL Server
xUnit (testing)
🔐 Authentication & Authorization

The API uses JWT Bearer authentication.

Features:
Secure login endpoint issuing JWT tokens
Role-based authorization (RBAC)
Protected endpoints using [Authorize]
Roles:
Admin
User
Authorization behavior:
Unauthorized requests → 401 Unauthorized
Forbidden role access → 403 Forbidden
📦 Key Features
🧩 CQRS Implementation
Commands handle write operations
Queries handle read operations
MediatR used exclusively in controllers (no direct service calls)
🧠 Domain Modeling (Polymorphism)

A key architectural improvement introduced polymorphic domain modeling:

MediaUnit (abstract base class)
PhysicalBookUnit
AudiobookUnit
Business rules enforced:
A media unit must be exactly one type
Cannot contain both page count and duration
Invalid combinations are rejected via validation layer
🔄 Validation Pipeline
Centralized FluentValidation pipeline using MediatR behavior
Validation executed before handlers
Ensures clean separation of concerns
🗺️ Mapping Strategy
Mapster used for DTO mapping
Explicit configuration for polymorphic entities
Ensures separation between domain and API models
🧾 Error Handling & Middleware
Centralized exception middleware
Standardized API error responses
Removed legacy filters for consistency
🧪 Testing
Unit and integration test suite (28+ tests)
Covers CQRS handlers, validation, and API behavior
Ensures stability after refactors
🗄️ Database
SQL Server (Code First)
Entity Framework Core
Migrations enabled
Automatic schema updates via migration pipeline
🚀 Getting Started
1. Clone repository
git clone <repo-url>
2. Configure database

Update appsettings.json:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=...;Trusted_Connection=True;"
  }
}
3. Apply migrations
dotnet ef database update
4. Run project
dotnet run
📖 API Documentation

Swagger available at:

/swagger

Supports JWT authentication testing directly in UI.

📁 Project Structure
API/
Application/
Domain/
Infrastructure/
🔄 Development Highlights

This project was developed through multiple structured improvements:

🔹 Architecture Refactoring
Standardized Clean Architecture layering
Removed legacy filters and inconsistent patterns
Enforced CQRS across all controllers
🔹 Authentication Overhaul
Implemented JWT authentication
Fixed token validation issues
Added role-based access control
Standardized authorization responses
🔹 Domain Evolution
Introduced polymorphic MediaUnit model
Refactored database schema accordingly
Updated handlers, DTOs, and validation logic
🔹 Codebase Stabilization
Fixed broken dependencies after refactors
Restored missing interfaces and DI configuration
Aligned test suite with updated architecture
🧠 Summary

This project demonstrates a fully implemented Clean Architecture backend with a strong focus on:

scalability
separation of concerns
secure authentication
maintainable CQRS structure
iterative refactoring and architectural improvement
📌 Notes
Mapster is used intentionally instead of AutoMapper for lightweight and explicit mapping control
The project has been continuously refactored to enforce architectural consistency
All major changes are reflected through structured pull requests
