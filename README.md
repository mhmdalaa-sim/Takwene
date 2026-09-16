# Takwene - Track Management

Local development instructions

Prerequisites:
- .NET 10 SDK
- (Optional) sqlite3 client

Run API (backend):
1. From repository root: dotnet restore
2. Build: dotnet build
3. Run: dotnet run --project .\Takwene.csproj
4. The API listens on the console (e.g., http://localhost:5128). The project uses a local SQLite file `takwene.db` created automatically.

Migrations (optional — recommended for production):
1. Install EF tools if not installed: dotnet tool install --global dotnet-ef
2. From repository root run:
   dotnet ef migrations add Initial --project .\Takwene.csproj
   dotnet ef database update --project .\Takwene.csproj
Note: the project currently uses EnsureCreated() for demo seeding; running migrations will create a migration scaffold you can commit.

Seeded data:
- The app seeds 3 artists, 8 tracks (various genres/statuses), 3 DSPs and sample distributions on first run.

JWT token (demo):
- Obtain a demo token by POST to `/api/auth/token` with JSON body:
  {
    "username": "demo",
    "password": "demo"
  }
- Example using curl:
  curl -X POST http://localhost:5128/api/auth/token -H "Content-Type: application/json" -d "{\"username\":\"demo\",\"password\":\"demo\"}"
- Use the returned token in the Authorization header for protected endpoints:
  Authorization: Bearer <token>
  e.g. POST /api/tracks/{id}/distribute

Client app (frontend):
- A simple React SPA is available at / (served from the ClientApp folder). The app is intentionally lightweight and uses the browser fetch API to call the backend.
- For production build with a toolchain, replace ClientApp with a built frontend and ensure static files are placed under ClientApp (Program.cs serves ClientApp static files).

Notes:
- DECISIONS.md contains notes about AI usage, security and fixes.

