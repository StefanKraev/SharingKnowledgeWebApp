# SharingKnowledgeWebApp
# 🚀 Sharing Knowledge - Where everyone can learn

> This project empowers students to engage in peer-to-peer learning through a library of student-led open courses!

![.NET Version](https://img.shields.io/badge/.NET-10.0-purple)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-10.0-blue)
![License](https://img.shields.io/badge/license-MIT-green)

---

## 📋 Table of Contents

- [About the Project](#about-the-project)
- [Technologies Used](#technologies-used)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Features](#features)
- [Usage](#usage)
- [Database Setup](#database-setup)
- [Configuration](#configuration)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

---

## 📖 About the Project

I developed this project to master the core concepts of ASP.NET, specifically focusing on MVC architecture, Razor Pages, and server-side rendering. The platform is designed for peer-to-peer education, enabling students to both share their expertise and learn from the experiences of others. Users have the autonomy to create, manage, and enroll in a variety of open courses.

---

## 🛠️ Technologies Used

|                      Technology                     | Version  |            Purpose               |
|-----------------------------------------------------|----------|----------------------------------|
| ASP.NET Core MVC                                    | 10.0     | Web framework                    |
| Entity Framework Core                               | 10.0     | ORM / Database access            |
| SQL Server                                          | 2019     | Database                         |
| Bootstrap                                           | 5.3      | Frontend styling                 |
| Razor Pages / Views                                 | 10.0     | Server-side HTML rendering       |
| Nugget packages:                                    |    --    |                                  |
| Microsoft.AspNetCore.Identity.EntityFrameworkCore   | 10.0.2   | Integrates ASP.NET Core Identity |
| Microsoft.EntityFrameworkCore.SqlServer             | 10.0.2   | Database Provider                |
| Microsoft.EntityFrameworkCore.Tools                 | 10.0.2   | Enables PowerShell commands      |
| jQuery                                              | 3.7.1    | Simplifies client interactivity  |

---

## ✅ Prerequisites

Make sure you have the following installed before running the project:

- [.NET SDK 10.0+](https://dotnet.microsoft.com/download)
- [Visual Studio 2026](https://visualstudio.microsoft.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- [Git](https://git-scm.com/)

---

## 🚀 Getting Started

Follow these steps to get the project running locally.

### 1. Clone the repository

```bash
git clone https://github.com/StefanKraev/SharingKnowledgeWebApp.git
cd SharingKnowledgeWebApp
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Apply database migrations

```bash
dotnet ef database update
```

### 4. Run the application

```bash
dotnet run
```

The app will be available at `https://localhost:7069`.

---

## 📁 Project Structure

```
YourProjectName/
│
├── Controllers/          # MVC Controllers
├── Models/               # Domain models and ViewModels
├── Views/                # Razor Views (.cshtml)
├── Data/                 # DbContext and migrations
├── Services/             # Business logic / service layer
├── wwwroot/              # Static files (CSS, JS, images)
├── appsettings.json      # App configuration
└── Program.cs            # App entry point and middleware setup
```

---

## ✨ Features

- [ ] User registration and login (ASP.NET Identity)
- [ ] CRUD operations for [OpenCourse]
- [ ] RESTful API endpoints
- [ ] Input validation (server-side & client-side)
- [ ] Responsive UI with Bootstrap

---

## 💻 Usage

```
1. Navigate to /Register to create an account.
2. Log in at /Login.
3.0 Click the section of the navbar named "Show me the courses!".
3.1 Click the button "View Details" on one of the courses to see details.
3.2 Click the button "Enroll" on one of the courses to add a course to your list of enrolled courses.
4.0 Click the section of the navbar named "Create new open course!" to access a form to create a new course.
4.1 After populating the from with valid data click on button "Create" to create a new Course.
5.0 Click the section of the navbar named "Show my courses" view all enrolled courses.
5.1 Click the button "Leave" on one of the courses to remove a course from the enrolled courses.
6 To delete a created course go to section "Show me the courses!" and click on the button bellow "Delete". Students who have enrolled this course will automatically lose this course from "My Courses".

```

---

## 🗄️ Database Setup

The project uses **Entity Framework Core** with a Code-First approach.

Connection string is configured in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb);Database=YourDbName;Trusted_Connection=True;Encrypt=False"
}
```

To create and seed the database:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

## ⚙️ Configuration

Key settings in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your-connection-string-here"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

> ⚠️ **Never commit sensitive data** (passwords, API keys) to source control. Use `appsettings.Development.json` or environment variables for local secrets!

---

## 🤝 Contributing

Contributions are welcome! To contribute:

1. Fork the repository
2. Sync your local main branch: `git pull origin main`
3. Create a new branch: `git checkout -b feature/your-feature-name`
4. Commit your changes: `git commit -m "Add some feature"`
5. Push to the branch: `git push origin feature/your-feature-name`
6. Open a Pull Request

---

## 📄 License

This project is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for details.

---

## 📬 Contact

**Stefan Kreav** – [@StefanKraev](https://github.com/StefanKraev)

Project Link: [https://github.com/StefanKraev/SharingKnowledgeWebApp](https://github.com/StefanKraev/SharingKnowledgeWebApp)

---

*Built as part of the **ASP.NET Fundamentals** course.*
