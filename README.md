# Takwene - Track Management

Local development instructions

Prerequisites:
- .NET 10 SDK
- (Optional) sqlite3 client

Run API:
1. From repository root: dotnet restore
2. dotnet run --project .\Takwene.csproj
3. The API listens on the default Kestrel port (see console). The project uses a local SQLite file `takwene.db` created automatically.

Migrations:
- This project uses EnsureCreated() for demo seeding. For full migrations locally, add EF Core tools and run `dotnet ef migrations add Initial` and `dotnet ef database update`.

Seeded data:
- 3 artists, 8 tracks, 3 DSPs and sample distributions are seeded on first run.

JWT token:
- Obtain a demo token by POST to `/api/auth/token` with JSON body { "username": "demo", "password": "demo" }.
- Use the returned token in Authorization: Bearer <token> to call protected endpoints (e.g., POST /api/tracks/{id}/distribute).

Client app:
- A simple React-based SPA is available at / (served from ClientApp folder). Open the API root in a browser to use it.

DECISIONS.md contains notes about AI usage, fixes and security considerations.

