# CarLeasingSystem

A professional-grade, web-based Car Leasing System built with ASP.NET Core MVC. This application provides a comprehensive platform for managing a car fleet, handling user authentication, and facilitating a seamless car booking and rental lifecycle.

## Features

### Admin Dashboard
- **Fleet Management:** Full CRUD (Create, Read, Update, Delete) functionality for the car catalog.
- **Booking Overview:** Centralized dashboard to view all customer bookings and manage fleet status.

### Customer Features
- **Search & Filter:** Intelligent search engine to find available cars based on user-selected date ranges.
- **Booking Workflow:**
    - **Book:** Secure booking process with automated availability checking (prevents double-booking).
    - **Cancel:** Users can securely cancel their existing bookings.
    - **Reschedule:** Built-in functionality to modify booking dates without losing reservation history.
- **My Bookings:** A dedicated dashboard for users to track their rental history.

### Technical Highlights
- **ASP.NET Core Identity:** Secure user authentication and authorization (Admin/User roles).
- **Entity Framework Core:** Robust data persistence with SQL Server/LocalDB.
- **Reusable UI Components:** Extracted partial views (e.g., `_CarCard`) for consistent, maintainable UI components.
- **Data Validation:** Server-side logic and Data Annotations to ensure data integrity (date range validation, non-overlapping intervals).

## 🛠 Tech Stack
- **Framework:** ASP.NET Core MVC (.NET 6+)
- **Database:** Entity Framework Core
- **Frontend:** Bootstrap 5, Razor Views
- **Language:** C#

## Getting Started

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or VS Code
- SQL Server (or SQL Server Express LocalDB)

### Setup
1. **Clone the repository:**
   ```bash
   git clone [https://github.com/amohnice/CarLeasingSystem.git](https://github.com/amohnice/CarLeasingSystem.git)

```

2. **Configure Database:**
* Open `appsettings.json`.
* Update the `ConnectionStrings:DefaultConnection` to point to your local SQL instance.


3. **Apply Migrations:**
* Open the Package Manager Console in Visual Studio and run:
```powershell
Update-Database

```




4. **Run the Application:**
* Press `F5` in Visual Studio or run `dotnet run` in the terminal.



## Project Structure

* `Controllers/`: Contains the logic for `Bookings`, `Cars`, and `Admin`.
* `Models/`: Database entities (`Car`, `Booking`, `User`).
* `Views/`: Razor templates for the UI, including the `Shared/_CarCard.cshtml` component.

## License

This project is for educational purposes.

---

*Built with care.*
