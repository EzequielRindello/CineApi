# Movie App Backend
API REST para gestión de funciones de películas con PostgreSQL y Entity Framework Core.

## Configuración del proyecto

### 1. Clonar el repositorio
### 2. Levantar la base de datos PostgreSQL

```bash
# Levantar el contenedor de PostgreSQL
docker-compose up -d

# Verificar que el contenedor esté corriendo
docker-compose ps
```

### 3. Instalar las dependencias de .NET

```bash
# Restaurar paquetes NuGet
dotnet restore
```

### 4. Crear y aplicar migraciones

```bash
# Instalar la herramienta EF Core (si no está instalada)
dotnet tool install --global dotnet-ef

# Crear la primera migración
dotnet ef migrations add InitialCreate

# Aplicar las migraciones a la base de datos
dotnet ef database update
```

## Endpoints disponibles

### Funciones de películas

- `GET /api/functions` - Obtener todas las funciones
- `GET /api/functions/{id}` - Obtener una función por ID
- `POST /api/functions` - Crear una nueva función
- `PUT /api/functions/{id}` - Actualizar una función
- `DELETE /api/functions/{id}` - Eliminar una función

### Películas (solo lectura)

- `GET /api/movies` - Obtener todas las películas
- `GET /api/movies/{id}` - Obtener una película por ID

## Estructura del proyecto

```
MovieApp/
├── Entity/                 # Entidades de la base de datos
│   ├── Director.cs
│   ├── Movie.cs
│   └── MovieFunction.cs
├── Models/                 # DTOs y modelos de request/response
│   ├── MovieDto.cs
│   └── MovieFunctionDto.cs
├── Services/               # Lógica de negocio
│   ├── MovieService.cs
│   └── MovieFunctionService.cs
├── Controllers/            # Controladores de la API
│   ├── MovieController.cs
│   └── MovieFunctionController.cs
├── Data/                   # Context de Entity Framework
│   └── AppDbContext.cs
├── Program.cs              # Configuración de la aplicación
├── appsettings.json        # Configuración de la aplicación
└── docker-compose.yml      # Configuración de Docker
```

## Ejemplo de uso

### Crear una función

```bash
curl -X POST https://localhost:5001/api/functions \
  -H "Content-Type: application/json" \
  -d '{
    "movieId": 1,
    "date": "2025-06-26T00:00:00",
    "time": "14:00:00",
    "price": 2500
  }'
```

## Configuración para el Frontend

El backend está configurado para aceptar peticiones CORS desde:
- `http://localhost:3000` (Create React App)
- `http://localhost:5173` (Vite)

Para conectar tu frontend React, usa la URL base: `https://localhost:5001/api`
