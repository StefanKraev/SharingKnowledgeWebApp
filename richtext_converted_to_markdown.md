🚀 Sharing Knowledge - Peer-to-Peer Learning Platform
=====================================================

> An advanced ASP.NET Core MVC platform where students empower each other through a library of student-led open courses and shared resources.

📋 Table of Contents
--------------------

*   [About the Project](https://github.com/StefanKraev/SharingKnowledgeWebApp#about-the-project)
    
*   [Technologies Used](https://github.com/StefanKraev/SharingKnowledgeWebApp#technologies-used)
    
*   Architecture and Design Desicions
    
*   [Prerequisites](https://github.com/StefanKraev/SharingKnowledgeWebApp#prerequisites)
    
*   [Getting Started](https://github.com/StefanKraev/SharingKnowledgeWebApp#getting-started)
    
*   [Project Structure](https://github.com/StefanKraev/SharingKnowledgeWebApp#project-structure)
    
*   [Features](https://github.com/StefanKraev/SharingKnowledgeWebApp#features)
    
*   Security and Validations
    
*   [Usage](https://github.com/StefanKraev/SharingKnowledgeWebApp#usage)
    
*   [Database Setup](https://github.com/StefanKraev/SharingKnowledgeWebApp#database-setup)
    
*   Test Covarage
    
*   [Configuration](https://github.com/StefanKraev/SharingKnowledgeWebApp#configuration)
    
*   [Contributing](https://github.com/StefanKraev/SharingKnowledgeWebApp#contributing)
    
*   [License](https://github.com/StefanKraev/SharingKnowledgeWebApp#license)
    
*   [Contact](https://github.com/StefanKraev/SharingKnowledgeWebApp#contact)
    

📖 About the Project
--------------------

I developed this project to master the core concepts of ASP.NET, specifically focusing on MVC architecture, Razor Pages, and server-side rendering. The platform is designed for peer-to-peer education, enabling students to both share their expertise and learn from the experiences of others. Users have the autonomy to create, manage, and enroll in a variety of open courses and share resources they find usefull.

🛠️ Technologies Used
---------------------

**TechnologyVersionPurpose**ASP.NET Core MVC10.0Web frameworkEntity Framework Core10.0ORM / Database accessSQL Server2019DatabaseBootstrap5.3Frontend stylingRazor Pages / Views10.0Server-side HTML renderingNugget packages:--Microsoft.AspNetCore.Identity.EntityFrameworkCore10.0.2Integrates ASP.NET Core IdentityMicrosoft.EntityFrameworkCore.SqlServer10.0.2Database ProviderMicrosoft.EntityFrameworkCore.Tools10.0.2Enables PowerShell commandsjQuery3.7.1Simplifies client interactivity

🏗️ Architecture & Design Decisions
-----------------------------------

### 1\. N-Tier Architecture

The project follows a clean separation of concerns to ensure maintainability and testability:

*   **Web Layer (MVC):** Handles HTTP requests, User Identity, and UI rendering via Razor.
    
*   **Service Layer (Core):** Contains the "Brain" of the application. Business rules, data mapping, and validation logic reside here.
    
*   **Data Layer (Repository Pattern):** Abstracts the database logic. This allows the Service layer to remain agnostic of the specific ORM or database provider used.
    

### 2\. Design Patterns & Principles

*   **Repository Pattern:** Decouples the business logic from Entity Framework Core. \* **Dependency Injection (DI):** All services and repositories are injected via the Program.cs to promote loose coupling.
    
*   **ViewModel Pattern:** Prevents over-posting attacks and decouples Database Entities from the UI.
    
*   **Wrapper Pattern:** Uses ApplicationUser as a wrapper around IdentityUser to allow for flexible identity extension.
    

✅ Prerequisites
---------------

Make sure you have the following installed before running the project:

*   [.NET SDK 10.0+](https://dotnet.microsoft.com/download)
    
*   [Visual Studio 2026](https://visualstudio.microsoft.com/)
    
*   [SQL Server](https://www.microsoft.com/en-us/sql-server)
    
*   [Git](https://git-scm.com/)
    

🚀 Getting Started
------------------

Follow these steps to get the project running locally.

### 1\. Clone the repository

Plain textANTLR4BashCC#CSSCoffeeScriptCMakeDartDjangoDockerEJSErlangGitGoGraphQLGroovyHTMLJavaJavaScriptJSONJSXKotlinLaTeXLessLuaMakefileMarkdownMATLABMarkupObjective-CPerlPHPPowerShell.propertiesProtocol BuffersPythonRRubySass (Sass)Sass (Scss)SchemeSQLShellSwiftSVGTSXTypeScriptWebAssemblyYAMLXML`   git clone https://github.com/StefanKraev/SharingKnowledgeWebApp.git  cd SharingKnowledgeWebApp   `

### 2\. Restore dependencies

Plain textANTLR4BashCC#CSSCoffeeScriptCMakeDartDjangoDockerEJSErlangGitGoGraphQLGroovyHTMLJavaJavaScriptJSONJSXKotlinLaTeXLessLuaMakefileMarkdownMATLABMarkupObjective-CPerlPHPPowerShell.propertiesProtocol BuffersPythonRRubySass (Sass)Sass (Scss)SchemeSQLShellSwiftSVGTSXTypeScriptWebAssemblyYAMLXML`   dotnet restore   `

### 3\. Apply database migrations

Plain textANTLR4BashCC#CSSCoffeeScriptCMakeDartDjangoDockerEJSErlangGitGoGraphQLGroovyHTMLJavaJavaScriptJSONJSXKotlinLaTeXLessLuaMakefileMarkdownMATLABMarkupObjective-CPerlPHPPowerShell.propertiesProtocol BuffersPythonRRubySass (Sass)Sass (Scss)SchemeSQLShellSwiftSVGTSXTypeScriptWebAssemblyYAMLXML`   dotnet ef database update   `

### 4\. Run the application

Plain textANTLR4BashCC#CSSCoffeeScriptCMakeDartDjangoDockerEJSErlangGitGoGraphQLGroovyHTMLJavaJavaScriptJSONJSXKotlinLaTeXLessLuaMakefileMarkdownMATLABMarkupObjective-CPerlPHPPowerShell.propertiesProtocol BuffersPythonRRubySass (Sass)Sass (Scss)SchemeSQLShellSwiftSVGTSXTypeScriptWebAssemblyYAMLXML`   dotnet run   `

The app will be available at https://localhost:7069.

📁 Project Structure
--------------------

Plain textANTLR4BashCC#CSSCoffeeScriptCMakeDartDjangoDockerEJSErlangGitGoGraphQLGroovyHTMLJavaJavaScriptJSONJSXKotlinLaTeXLessLuaMakefileMarkdownMATLABMarkupObjective-CPerlPHPPowerShell.propertiesProtocol BuffersPythonRRubySass (Sass)Sass (Scss)SchemeSQLShellSwiftSVGTSXTypeScriptWebAssemblyYAMLXML`   SharingKnowledgeWebApp/  │  ├── Controllers/          # MVC Controllers  ├── Models/               # Domain models and ViewModels  ├── Views/                # Razor Views (.cshtml)  ├── Data/                 # DbContext and migrations  ├── Services/             # Business logic / service layer  ├── wwwroot/              # Static files (CSS, JS, images)  ├── appsettings.json      # App configuration  └── Program.cs            # App entry point and middleware setup   `

✨ Features
----------

*    User registration and login (ASP.NET Identity)
    
*    CRUD operations for \[OpenCourse & Book\]
    
*    RESTful API endpoints
    
*    Input validation (server-side & client-side)
    
*    Responsive UI with Bootstrap
    
*   Search and Filter functionality for courses
    

### 🛡️ Security & Validation

*   **ASP.NET Core Identity:** Secure authentication and role-based authorization (Admin vs. Student).
    
*   **Anti-Forgery:** Implementation of \[ValidateAntiForgeryToken\] on all POST actions.
    
*   **Server-Side Validation:** Data annotations and custom logic ensure data integrity before database persistence.
    

💻 Usage
--------

Plain textANTLR4BashCC#CSSCoffeeScriptCMakeDartDjangoDockerEJSErlangGitGoGraphQLGroovyHTMLJavaJavaScriptJSONJSXKotlinLaTeXLessLuaMakefileMarkdownMATLABMarkupObjective-CPerlPHPPowerShell.propertiesProtocol BuffersPythonRRubySass (Sass)Sass (Scss)SchemeSQLShellSwiftSVGTSXTypeScriptWebAssemblyYAMLXML`   1. Navigate to /Register to create an account.  2. Log in at /Login.  3.0 Click the section of the navbar named "Show me the courses!".  3.1 Click the button "View Details" on one of the courses to see details.  3.2 Click the button "Enroll" on one of the courses to add a course to your list of enrolled courses.  4.0 Click the section of the navbar named "Create new open course!" to access a form to create a new course.  4.1 After populating the from with valid data click on button "Create" to create a new Course.  5.0 Click the section of the navbar named "Show my courses" view all enrolled courses.  5.1 Click the button "Leave" on one of the courses to remove a course from the enrolled courses.  6 To delete a created course go to section "Show me the courses!" and click on the button bellow "Delete". Students who have enrolled this course will automatically lose this course from "My Courses".  7.0 Click on “Show me the books” on the navigation bar to display all registered books.  7.1 Click on the button "Details" on the book entity in the index page to access books details.  7.2 Click on "Create a new book" on the navigation bar to create a new book.  7.3 If you are the creater of the book after clicking "Details" buttons Edit and Delete are available.   `

🧪 Test Coverage
----------------

The project includes a dedicated Test Project focusing on high-impact logic:

*   **Service Tests:** Validating mapping logic, ownership checks, and database interaction.
    
*   **Controller Tests:** Mocking HttpContext and ClaimsPrincipal to test secure actions like Enroll and Edit.
    

🗄️ Database Setup
------------------

The project uses **Entity Framework Core** with a Code-First approach.

Connection string is configured in appsettings.json:

Plain textANTLR4BashCC#CSSCoffeeScriptCMakeDartDjangoDockerEJSErlangGitGoGraphQLGroovyHTMLJavaJavaScriptJSONJSXKotlinLaTeXLessLuaMakefileMarkdownMATLABMarkupObjective-CPerlPHPPowerShell.propertiesProtocol BuffersPythonRRubySass (Sass)Sass (Scss)SchemeSQLShellSwiftSVGTSXTypeScriptWebAssemblyYAMLXML`   "ConnectionStrings": {    "DefaultConnection": "Server=(localdb);Database=YourDbName;Trusted_Connection=True;Encrypt=False"  }   `

To create and seed the database:

Plain textANTLR4BashCC#CSSCoffeeScriptCMakeDartDjangoDockerEJSErlangGitGoGraphQLGroovyHTMLJavaJavaScriptJSONJSXKotlinLaTeXLessLuaMakefileMarkdownMATLABMarkupObjective-CPerlPHPPowerShell.propertiesProtocol BuffersPythonRRubySass (Sass)Sass (Scss)SchemeSQLShellSwiftSVGTSXTypeScriptWebAssemblyYAMLXML`   dotnet ef migrations add InitialCreate  dotnet ef database update   `

⚙️ Configuration
----------------

Key settings in appsettings.json:

Plain textANTLR4BashCC#CSSCoffeeScriptCMakeDartDjangoDockerEJSErlangGitGoGraphQLGroovyHTMLJavaJavaScriptJSONJSXKotlinLaTeXLessLuaMakefileMarkdownMATLABMarkupObjective-CPerlPHPPowerShell.propertiesProtocol BuffersPythonRRubySass (Sass)Sass (Scss)SchemeSQLShellSwiftSVGTSXTypeScriptWebAssemblyYAMLXML`   {    "ConnectionStrings": {      "DefaultConnection": "*"    },    "Logging": {      "LogLevel": {        "Default": "Information",        "Microsoft.AspNetCore": "Warning"      }    },    "AllowedHosts": "*",    "IdentityOptions": {      "SignIn": {        "RequiredConfirmedEmail": false,        "RequiredConfirmedAccount": false      },      "Lockout": {        "MaxFailedAttempts": 5,        "DefaultLockoutTimeSpan": "00:10:00"      },      "Password": {        "RequireDigit": true,        "RequireUppercase": true,        "RequireLowercase": true,        "RequireNonAlphanumeric": true,        "RequiredLength": 6,        "RequiredUniqueChars": 2      }    }  }   `

> ⚠️ **Never commit sensitive data** (passwords, API keys) to source control. Use appsettings.Development.json or environment variables for local secrets!

🤝 Contributing
---------------

Contributions are welcome! To contribute:

1.  Fork the repository
    
2.  Sync your local main branch: git pull origin main
    
3.  Create a new branch: git checkout -b feature/your-feature-name
    
4.  Commit your changes: git commit -m "Add some feature"
    
5.  Push to the branch: git push origin feature/your-feature-name
    
6.  Open a Pull Request
    

📄 License
----------

This project is licensed under the **MIT License**. See the [LICENSE](https://github.com/StefanKraev/SharingKnowledgeWebApp/blob/master/LICENSE) file for details.

📬 Contact
----------

**Stefan Kreav** – [@StefanKraev](https://github.com/StefanKraev)

Project Link: [https://github.com/StefanKraev/SharingKnowledgeWebApp](https://github.com/StefanKraev/SharingKnowledgeWebApp)

_Built as part of the_ _**ASP.NET Fundamentals**_ _course._