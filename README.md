# 🎓 EduNextLevel – ASP.NET Core Educational Platform

EduNextLevel is a web-based platform designed for publishing and organizing educational materials for students in grades 1–12. The application supports role-based access and allows users to manage articles, comments, classes, and subject categories.

## 🧩 Key Features

- Public access to articles without authentication
- User authentication and registration
- Logged-in users can post comments
- Moderators can delete inappropriate comments
- Admins can add/edit articles, classes, and topics
- Articles can be reassigned to different categories

## 🛠️ Technologies Used

- **Backend**: ASP.NET Core MVC, C#
- **Authentication**: ASP.NET Core Identity
- **Frontend**: Razor Pages, HTML, CSS
- **Database**: Microsoft SQL Server
- **ORM**: Entity Framework Core
- **Persistence**: LINQ + EF Core + Microsoft.EntityFrameworkCore.SqlServer

## ⚙️ Running Locally

### 🔧 Requirements

- Visual Studio 2022 or newer
- .NET 6.0 SDK or higher
- Microsoft SQL Server
- Entity Framework Core
- ASP.NET Core Identity

### 📝 Steps

1. Clone the repository:

    ```bash
    git clone https://github.com/laur004/EduNextLevel.git
    ```

2. Open the solution `EduNextLevel.sln` in Visual Studio.

3. Configure the database connection string in `appsettings.json`:

    ```json
    "ConnectionStrings": {
      "DefaultConnection": "User Id=USERNAME;Password=PASSWORD;Data Source=localhost:1521/XEPDB1"
    }
    ```

4. Apply the database migration:
    - In Visual Studio, go to:
      `Tools` → `NuGet Package Manager` → `Package Manager Console`
    - Run the following command:

    ```powershell
    Update-Database
    ```

5. Start the application:
    - Press `F5` or click the `Start` button in Visual Studio

## 📸 Screenshots

![Articles-Index](https://github.com/user-attachments/assets/cc1ff83a-e581-4711-b57a-613d81b7e6ea)

![Article](https://github.com/user-attachments/assets/4bc6f312-88ee-45cb-94fc-7b3f554fd43b)

![Admin Panel](https://github.com/user-attachments/assets/9ae7fab8-7980-4d7a-9594-7594f86be10e)


