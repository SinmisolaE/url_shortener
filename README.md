
# URLShort

A simple URL Shortener API built with ASP.NET Core.

## Project Structure

- `URLShort.API/` - Main API project (controllers, DTOs, services)
- `URLShort.Core/` - Core logic, entities, and interfaces
- `URLShort.Infrastructure/` - Database context, migrations, and repository implementations

## Features
- Shorten long URLs to short codes
- Redirect short codes to original URLs
- RESTful API endpoints
- Entity Framework Core for data access
- Layered architecture (API, Core, Infrastructure)

## Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- mysql 

### Setup
1. Clone the repository:
	```bash
	git clone <repo-url>
	cd URLShort
	```
2. Restore dependencies:
	```bash
	dotnet restore
	```
3. Apply database migrations:
	```bash
	dotnet ef database update --project URLShort.Infrastructure --startup-project URLShort.API
	```
4. Run the API:
	```bash
	dotnet run --project URLShort.API
	```

### API Endpoints
- `POST /api/url/shorten` - Shorten a long URL
- `GET /api/url/{shortCode}` - Redirect to the original URL

### Project Structure Details
- **Controllers**: Handle HTTP requests (see `URLShort.API/Controllers/`)
- **DTOs**: Data transfer objects for requests/responses (see `URLShort.API/DTO/`)
- **Services**: Business logic (see `URLShort.API/Service/`)
- **Entities**: Database models (see `URLShort.Core/Entities/`)
- **Repositories**: Data access layer (see `URLShort.Infrastructure/Repository/`)
- **DbContext**: EF Core context (see `URLShort.Infrastructure/Data/ShortenUrlDbContext.cs`)

## Development
- To add a new migration:
  ```bash
  dotnet ef migrations add <MigrationName> --project URLShort.Infrastructure --startup-project URLShort.API
  ```
- To update the database:
  ```bash
  dotnet ef database update --project URLShort.Infrastructure --startup-project URLShort.API
  ```

## License
MIT
