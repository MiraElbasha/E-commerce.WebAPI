# 🛒 E-Commerce RESTful Web API (.NET Core)

A robust, scalable, and maintainable E-Commerce Backend RESTful API built using **ASP.NET Core Web API** following **Onion Architecture** and modern software engineering practices.

---

## 🚀 Key Features & Modules

- **Authentication & Authorization:** Secure user authentication using **ASP.NET Core Identity** and **JWT (JSON Web Tokens)**.
- **Products Module:** Product browsing with advanced dynamic querying features.
- **Advanced Querying:** Built-in support for **Paging, Filtering, Sorting, and Searching** using the **Specification Pattern**.
- **Basket & Caching:** High-performance caching and distributed memory management using **Redis**.
- **Orders Module:** Complete order lifecycle management.
- **Payment Gateway Integration:** Integrated payment processing flow for checkout.
- **Global Error Handling:** Centralized handling for consistent error responses across all API endpoints.
- **API Documentation:** Interactive documentation using **Swagger UI** and **Postman**.

---

## 🛠️ Architecture & Design Patterns

This project adheres to **Clean Architecture / Onion Architecture** principles to promote separation of concerns, testability, and maintainability:

- **Onion Architecture:** Separation of Domain, Application, Infrastructure, and API layers.
- **Generic Repository Pattern:** Abstraction over data access layer.
- **Unit of Work Pattern:** Ensures data integrity and transaction management across multiple repositories.
- **Specification Pattern:** Decouples query logic from repository implementations.
- **AutoMapper:** Object-to-object mapping for DTOs (Data Transfer Objects).

---

## 🧰 Tech Stack & Tools

- **Framework:** .NET Core / ASP.NET Core Web API
- **Language:** C#
- **Database:** SQL Server
- **ORM:** Entity Framework Core
- **Caching:** Redis
- **Security:** ASP.NET Core Identity, JWT Tokens
- **Documentation:** Swagger / Open API, Postman
- **Deployment:** IIS Ready

---

## ⚙️ Getting Started

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) (6.0 or later)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Redis Server](https://redis.io/download/) (or run via Docker)

### Installation & Setup

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/your-username/your-repo-name.git](https://github.com/MiraElbasha/E-commerce.WebAPI.git)(https://github.com/MiraElbasha/E-commerce.WebAPI.git)
