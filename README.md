# Order Management System & Analytics Platform

A production-ready .NET 8 Web API built with **Clean Architecture** and **Domain-Driven Design (DDD)** principles. The project simulates a high-performance order management and analytical reporting platform, showcasing advanced **Entity Framework Core**, **LINQ queries**, **SQL Server Stored Procedures**, and enterprise reporting capabilities (**SSRS**).

---

## 🏗 Architecture & Design Patterns

The solution strictly adheres to **Clean Architecture** to ensure low coupling, high testability, and clear separation of concerns.

```text
Solution Structure:
├── 1. Domain                   # Pure POCO Entities, Enums, and Core Domain Rules
├── 2. Application              # DTOs, Service Contracts, and LINQ Analytics Queries
├── 3. Infrastructure           # DbContext, Fluent API Configurations, Extensions
└── 4. WebApi (Presentation)    # API Controllers, Middleware, Composition Root

Key Architectural Highlights:
Dependency Inversion Principle (DIP): The Application layer depends only on abstractions (IApplicationDbContext), keeping business logic isolated from database frameworks.

Encapsulated Entity Configurations: Fluent API configurations (IEntityTypeConfiguration<T>) are decoupled from domain models to preserve domain purity.

Encapsulated Service Registration: Infrastructure dependency registration is encapsulated using IServiceCollection extension methods.

Read-Only Performance: Read operations use .AsNoTracking() to avoid unnecessary Change Tracker overhead in EF Core.

🚀 Key Features
Advanced Analytics Engine (EF Core & LINQ):

Multi-level aggregation calculating total spent, total items purchased, order count, and most popular category per customer within a sliding 90-day window.

Optimized projections (Select) avoiding the N+1 query problem and reducing memory consumption.

Server-side pagination using custom PaginatedResult<T> wrappers.

Enterprise Reporting (SQL Server & SSRS):

Stored procedure-driven analytical reports (sp_GetCustomerPerformanceReport) with parameterized filtering.

Integration-ready SSRS matrix and summary reporting setup.

Clean Code & Modern C# Standards:

Strict nullability checks (Nullable Reference Types enabled).

Explicit decimal precision configuring and database index planning.

🛠 Tech Stack & Tools
Framework: .NET 8 Web API

Language: C# 12

ORM: Entity Framework Core 8

Database: SQL Server

Reporting: SQL Server Reporting Services (SSRS) & T-SQL Stored Procedures

Documentation: Swagger / OpenAPI

Architecture: Clean Architecture, CQRS-friendly design, Repository/DbContext Abstraction

📊 Domain Model Overview
Customer: Stores customer profiles and region classification.

Order: Represents order headers with status tracking (Pending, Processing, Completed, Cancelled).

OrderItem: Detailed order line items featuring unit prices and discount calculations.

Product: Product catalog maintaining category metadata and inventory counts.

⚡ Getting Started
Prerequisites
.NET 8.0 SDK

SQL Server (LocalDB, Express, or Developer edition)
