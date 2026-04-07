🚀 Sharing Knowledge - Peer-to-Peer Learning Platform

An advanced ASP.NET Core MVC platform where students empower each other through a library of student-led open courses and shared resources.

---

## 📋 Table of Contents
- [About the Project](#-about-the-project)
- [Technologies Used](#️-technologies-used)
- [Architecture & Design Decisions](#️-architecture--design-decisions)
- [Prerequisites](#-prerequisites)
- [Getting Started](#-getting-started)
- [Project Structure](#-project-structure)
- [Features](#-features)
- [Security & Validation](#-security--validation)
- [Usage](#-usage)
- [Database Setup](#-database-setup)
- [Test Coverage](#-test-coverage)
- [Configuration](#-configuration)
- [Contributing](#-contributing)
- [License](#-license)
- [Contact](#-contact)

---

## 📖 About the Project
I developed this project to master the core concepts of ASP.NET, specifically focusing on MVC architecture, Razor Pages, and server-side rendering. The platform is designed for peer-to-peer education, enabling students to both share their expertise and learn from the experiences of others. Users have the autonomy to create, manage, and enroll in a variety of open courses and share resources they find useful.

---

## 🛠️ Technologies Used

### Core Stack
| Technology | Version | Purpose |
| :--- | :--- | :--- |
| **ASP.NET Core MVC** | 10.0 | Web framework |
| **Entity Framework Core** | 10.0 | ORM / Database access |
| **SQL Server** | 2019 | Database |
| **Bootstrap** | 5.3 | Frontend styling |
| **Razor Pages / Views** | 10.0 | Server-side HTML rendering |
| **jQuery** | 3.7.1 | Simplifies client interactivity |

### NuGet Packages
| Package Name | Version | Purpose |
| :--- | :--- | :--- |
| **Microsoft.AspNetCore.Identity.EntityFrameworkCore** | 10.0.2 | Integrates ASP.NET Core Identity |
| **Microsoft.EntityFrameworkCore.SqlServer** | 10.0.2 | Database Provider |
| **Microsoft.EntityFrameworkCore.Tools** | 10.0.2 | Enables PowerShell commands (Migrations) |

---

## 🏗️ Architecture & Design Decisions

### 1. N-Tier Architecture
The project follows a clean separation of concerns to ensure maintainability and testability:
* **Web Layer (MVC):** Handles HTTP requests, User Identity, and UI rendering via Razor.
* **Service Layer (Core):** Contains the business logic, data mapping, and validation.
* **Data Layer (Repository Pattern):** Abstracts the database logic, keeping the service layer agnostic of the specific ORM.

### 2. Design Patterns & Principles
* **Repository Pattern:** Decouples the business logic from Entity Framework Core.
* **Dependency Injection (DI):** Services and repositories are injected via `Program.cs` for loose coupling.
* **ViewModel Pattern:** Prevents over-posting attacks and separates Entities from the UI.
* **Wrapper Pattern:** Uses `ApplicationUser` as a wrapper around `IdentityUser` for flexible extension.

---

## ✅ Prerequisites
Make sure you have the following installed before running the project:
* .NET SDK 10.0+
* Visual Studio 2026
* SQL Server
* Git

---

## 🚀 Getting Started

1. **Clone the repository**
   ```bash
   git clone [https://github.com/StefanKraev/SharingKnowledgeWebApp.git](https://github.com/StefanKraev/SharingKnowledgeWebApp.git)
   cd SharingKnowledgeWebApp
Restore dependencies

Bash
dotnet restore
Apply database migrations

Bash
dotnet ef database update
Run the application

Bash
dotnet run
The app will be available at: https://localhost:7069

📁 Project Structure
Plaintext
SharingKnowledgeWebApp/
│
├── Controllers/          # MVC Controllers
├── Models/               # Domain models and ViewModels
├── Views/                # Razor Views (.cshtml)
├── Data/                 # DbContext and migrations
├── Services/             # Business logic / service layer
├── wwwroot/              # Static files (CSS, JS, images)
├── appsettings.json      # App configuration
└── Program.cs            # App entry point and middleware setup
✨ Features
✅ User registration and login (ASP.NET Identity)

✅ CRUD operations for OpenCourse & Book

✅ RESTful API endpoints

✅ Input validation (server-side & client-side)

✅ Responsive UI with Bootstrap

✅ Search and Filter functionality for courses

## 🛡️ Security & Validation
ASP.NET Core Identity: Secure authentication and role-based authorization.

Anti-Forgery: Implementation of [ValidateAntiForgeryToken] on all POST actions.

Server-Side Validation: Data annotations ensure data integrity before persistence.

## 💻 Usage
Register/Login: Navigate to /Register or /Login.

Browse: Click "Show me the courses!" in the navbar.

Enroll: View details of a course and click "Enroll".

Create: Use "Create new open course!" to share expertise.

Manage: View your learning progress in "Show my courses".

Admin/Creator: Edit or delete your courses. Deletion automatically unenrolls students.

Resources: Browse or contribute books via the "Show me the books" section.

🧪 Test Coverage
The project includes a dedicated Test Project:

Service Tests: Validating mapping, ownership checks, and DB interaction.

Controller Tests: Mocking HttpContext to test secure actions like Enroll and Edit.

🗄️ Database Setup
Configure your connection string in appsettings.json:

JSON
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb);Database=YourDbName;Trusted_Connection=True;Encrypt=False"
}
Apply migrations:

Bash
dotnet ef migrations add InitialCreate
dotnet ef database update
## ⚙️ Configuration
Example Identity settings in appsettings.json:

JSON
{
  "IdentityOptions": {
    "Password": {
      "RequireDigit": true,
      "RequiredLength": 6,
      "RequiredUniqueChars": 2
    },
    "Lockout": {
      "MaxFailedAttempts": 5,
      "DefaultLockoutTimeSpan": "00:10:00"
    }
  }
}
[!WARNING]
Never commit sensitive data to source control. Use Environment Variables for secrets.

## 🤝 Contributing
Fork the repository.

Create a branch: git checkout -b feature/your-feature-name.

Commit changes: git commit -m "Add some feature".

Push: git push origin feature/your-feature-name.

Open a Pull Request.

## 📄 License
This project is licensed under the MIT License.

## 📬 Contact
Stefan Kraev – @StefanKraev

Project Link: https://github.com/StefanKraev/SharingKnowledgeWebApp
