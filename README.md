# Movie App Backend

REST API for managing movie screenings using PostgreSQL and Entity Framework Core. Supports JWT authentication and role-based access control.

## Project Setup

### 1. Clone the repository

```bash
git clone https://github.com/your-username/movie-app-backend.git
cd movie-app-backend

# Start the PostgreSQL container
docker-compose up -d

# Check that the container is running
docker-compose ps

# Restore NuGet packages
dotnet restore

# Install EF Core CLI if not already installed
dotnet tool install --global dotnet-ef

# Create the initial migration
dotnet ef migrations add InitialCreate

# Apply the migration to the database
dotnet ef database update
```

⚠️ If the project already contains migrations, you may not need to run these commands.

## Backend base URL

```
https://localhost:5001/api
```

CORS is enabled for:

- http://localhost:3000 (Create React App)
- http://localhost:5173 (Vite)

## Implemented Features

### User Roles

There are three user roles:

| Role         | Permissions                                              |
|--------------|----------------------------------------------------------|
| Sys Admin    | Full access. Can manage users, movies, and screenings.   |
| Cinema Admin | Can manage movies and screenings.                        |
| Regular User | Can reserve up to 4 seats per screening.                 |

- All users log in through the same login form.
- Only regular users can register.
- Sys Admin is responsible for creating other admins and users via the user management module.

## Available Endpoints

### Auth

- `POST /api/auth` – Login
- `GET /api/auth/{id}` – Get user by ID
- `PUT /api/auth/{id}` – Update user
- `DELETE /api/auth/{id}` – Delete user

### Users

- `GET /api/users` – List all users (Sys Admin)
- `POST /api/users` – Create user (Sys Admin)
- `PUT /api/users/{id}` – Update user (Sys Admin)
- `DELETE /api/users/{id}` – Delete user (Sys Admin)

### Screenings

- `GET /api/functions` – Get all screenings
- `GET /api/functions/{id}` – Get screening by ID
- `POST /api/functions` – Create a screening (Sys/Cinema Admin)
- `PUT /api/functions/{id}` – Update screening (Sys/Cinema Admin)
- `DELETE /api/functions/{id}` – Delete screening (Sys/Cinema Admin)

### Movies

- `GET /api/movies` – List all movies
- `GET /api/movies/{id}` – Get movie by ID
- `POST /api/movies` – Create a movie (Sys/Cinema Admin)
- `PUT /api/movies/{id}` – Update movie (Sys/Cinema Admin)
- `DELETE /api/movies/{id}` – Delete movie (Sys/Cinema Admin)

### Reservations (Regular Users)

- `POST /api/reservations` – Reserve seats (max 4 per screening)
- `GET /api/reservations/user/{userId}` – Get user reservations
- `DELETE /api/reservations/{id}` – Cancel reservation

## Usage Example

### Create a screening

```bash
curl -X POST https://localhost:5001/api/functions \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <your_token>" \
  -d '{
    "date": "2025-07-03T00:00:00Z",
    "time": "14:30:00",
    "price": 12.50,
    "movieId": 1
}'
```
