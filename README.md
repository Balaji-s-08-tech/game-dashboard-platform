# Game Dashboard

A full-stack gaming dashboard application built with an ASP.NET Core backend and a React frontend.

## Architecture

- **Backend**: C# ASP.NET Core REST API
- **Frontend**: React.js
- **Database**: SQLite (local development)

## Setup and Run

### Prerequisites
- .NET 8 SDK
- Node.js & npm

### Backend

1. Navigate to the backend directory:
   ```bash
   cd backend/src/GameDataBackend.API
   ```
2. Run the API:
   ```bash
   dotnet run
   ```

### Frontend

1. Navigate to the frontend directory:
   ```bash
   cd frontend
   ```
2. Install dependencies:
   ```bash
   npm install
   ```
3. Run the development server:
   ```bash
   npm start
   ```

## API Endpoints

The backend exposes a REST API for game data metrics. Please refer to the Swagger UI at `http://localhost:5000/swagger` (or the configured port) when the backend is running.
