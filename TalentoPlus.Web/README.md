# TalentoPlus.Web

## Description
TalentoPlus.Web is the user-facing web application for the TalentoPlus platform. It is built using ASP.NET Core MVC and provides interfaces for managing employees, departments, and viewing dashboards.

## Key Features
- **Dashboard**: Visual overview of key metrics.
- **Employee Management**: Create, read, update, and delete employee records.
- **Authentication**: Secure login and role-based access control.

## Admin Credentials
The application comes with a pre-configured admin user for development and testing purposes.

- **Username**: `admin@talentoplus.com`
- **Password**: `Admin123.`

> **Note**: Please ensure to change these credentials or disable this account in a production environment.

## Running the Application
This project is configured to run within a Docker container as part of the solution's docker-compose setup.

If running locally without Docker:
1. Navigate to the project directory.
2. Run `dotnet run`.
3. Access the application at `http://localhost:5144` (or the port configured in launchSettings.json).
