# TalentoPlus.Api

## Description
TalentoPlus.Api is a RESTful API built with ASP.NET Core. It serves as the backend service for the TalentoPlus platform, handling data persistence and business logic.

## Documentation
The API documentation is available via Swagger UI when running in development mode.

- **Swagger UI**: `http://localhost:8081/swagger`

## Key Features
- **RESTful Endpoints**: Standardized API for resource management.
- **Authentication**: JWT-based authentication for secure access.
- **Data Access**: Entity Framework Core integration for database operations.

## Running the API
This project is configured to run within a Docker container as part of the solution's docker-compose setup.

If running locally without Docker:
1. Navigate to the project directory.
2. Run `dotnet run`.
3. Access the API at `http://localhost:5000` (or the port configured in launchSettings.json).
