# TalentoPlus S.A.S

## Overview
TalentoPlus S.A.S is a comprehensive software solution designed to manage company talent and operations. This repository contains the source code for the web application, API, and test suites.

## Project Structure
The solution consists of the following main projects:

- **TalentoPlus.Web**: The main web application built with ASP.NET Core MVC.
- **TalentoPlus.Api**: A RESTful API to expose data and functionality.
- **TalentoPlus.Tests**: Unit and integration tests to ensure code quality.

## Prerequisites
To run this project, ensure you have the following installed:

- Docker and Docker Compose
- .NET 8 SDK (for local development without Docker)

## Getting Started

### Running with Docker Compose
The easiest way to run the entire solution is using Docker Compose.

1. Open a terminal in the root directory of the project.
2. Run the following command:
   ```bash
   docker-compose up --build
   ```
3. The services will start in the configured order.

### Accessing the Application
- **Web Application**: http://localhost:8080
- **API**: http://localhost:8081
