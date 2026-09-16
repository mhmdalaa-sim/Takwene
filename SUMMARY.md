Summary of work performed:

- Added domain entities: Artist, Track, Dsp, TrackDistribution.
- Added EF Core AppDbContext with seeded sample data (3 artists, 8 tracks, 3 DSPs, distributions).
- Implemented application services (ArtistService, TrackService) and DTOs.
- Implemented API controllers: ArtistsController, TracksController, AuthController (demo JWT issuer).
- Configured Program.cs to use SQLite, JWT authentication, static file serving for the ClientApp.
- Created a simple React SPA (ClientApp) loaded via CDN that lists tracks and shows details.
- Added README.md and DECISIONS.md.
- Added necessary NuGet packages and started the API to verify seed and endpoints.

Next steps you may want to run locally:
- dotnet restore
- dotnet build
- dotnet run --project .\Takwene.csproj
- Open http://localhost:5128/ to see the ClientApp UI.
- Obtain a demo JWT by POSTing to /api/auth/token with {"username":"demo","password":"demo"} and use it to call POST /api/tracks/{id}/distribute.
