# Recruitment Portal

A full-stack recruitment portal with user **registration / login** and a
**candidate application form**.

- **Backend:** ASP.NET Core 8 Web API, **Dapper** + **Microsoft.Data.SqlClient**
  (MSSQL), **DB-first** approach, JWT authentication.
- **Frontend:** Angular 18 (standalone components) with login, registration
  and a candidate form.
- **Database:** SQL Server (`Users` + `Candidates` tables).

```
recruitment-portal/
├── database/
│   └── schema.sql                # DB-first: creates DB + tables
├── backend/
│   ├── RecruitmentPortal.sln
│   └── RecruitmentPortal.API/    # ASP.NET Core 8 Web API
└── frontend/                     # Angular 18 app
```

---

## 1. Database (DB-first)

The backend uses a **database-first** approach — create the schema first,
then the API maps to it with Dapper.

1. Start a SQL Server instance (local, Docker, or Azure SQL).
   Docker example:
   ```bash
   docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Your_password123" \
     -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
   ```
2. Run the schema script:
   ```bash
   sqlcmd -S localhost -U sa -P Your_password123 -i database/schema.sql
   ```
   (or open `database/schema.sql` in SSMS / Azure Data Studio and execute it.)

This creates the `RecruitmentPortal` database with `Users` and `Candidates`
tables.

---

## 2. Backend API

**Requirements:** [.NET 8 SDK](https://dotnet.microsoft.com/download)

1. Update the connection string and JWT key in
   `backend/RecruitmentPortal.API/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=RecruitmentPortal;User Id=sa;Password=Your_password123;TrustServerCertificate=True;Encrypt=True;"
   },
   "Jwt": { "Key": "<a long random secret, at least 32 chars>" }
   ```
2. Restore & run:
   ```bash
   cd backend/RecruitmentPortal.API
   dotnet restore
   dotnet run
   ```
3. The API starts at `http://localhost:5000` with Swagger UI at
   `http://localhost:5000/swagger`.

### Endpoints

| Method | Route                    | Auth  | Description                    |
|--------|--------------------------|-------|--------------------------------|
| POST   | `/api/auth/register`     | No    | Register a new user            |
| POST   | `/api/auth/login`        | No    | Login, returns a JWT           |
| GET    | `/api/candidates`        | JWT   | List all candidate applications|
| GET    | `/api/candidates/{id}`   | JWT   | Get one candidate              |
| POST   | `/api/candidates`        | JWT   | Submit a candidate application |
| PUT    | `/api/candidates/{id}`   | JWT   | Update a candidate application |
| DELETE | `/api/candidates/{id}`   | JWT   | Delete a candidate application |

Passwords are stored as PBKDF2 (SHA-256) salted hashes; protected endpoints
require a `Bearer` JWT.

---

## 3. Frontend (Angular)

**Requirements:** Node.js 18+ and npm.

```bash
cd frontend
npm install
npm start
```

The app runs at `http://localhost:4200` and talks to the API at
`http://localhost:5000/api` (configured in
`src/environments/environment.ts`).

### Flow
1. **Register** a new account (`/register`) or **Login** (`/login`).
2. On success you land on the **Dashboard** (`/dashboard`) which lists
   candidate applications.
3. Click **New Application** to open the **Candidate Form**
   (`/candidate/new`), fill it in, and submit.
4. Existing applications can be edited or deleted from the dashboard.

The JWT is stored in `localStorage` and attached to API calls by an HTTP
interceptor; an `authGuard` protects the dashboard and form routes.

---

## Tech summary

| Layer     | Technology                                           |
|-----------|------------------------------------------------------|
| Frontend  | Angular 18, standalone components, Reactive Forms     |
| Backend   | ASP.NET Core 8 Web API                                |
| Data      | Dapper + Microsoft.Data.SqlClient (DB-first)          |
| Auth      | JWT Bearer tokens, PBKDF2 password hashing            |
| Database  | Microsoft SQL Server                                  |
