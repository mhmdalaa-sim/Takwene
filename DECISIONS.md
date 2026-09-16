DECISIONS

1) What did AI generate and what was hand-written?
- AI-assisted: The assistant helped scaffold the project structure, generate domain entities, DTOs, DbContext with seed data, controllers, and service method stubs. I iterated on the generated code to fix namespaces, add validation, and wire up DI.
- Hand-written: I implemented service validation logic, the seeded sample data values, Program.cs wiring for static file serving and JWT configuration, and the lightweight React client (ClientApp) for the front-end UI.

2) What security issues did you find (or introduce) in the AI-generated code? How did you handle them?
- Demo JWT security: The AuthController issues tokens without credential checks and the symmetric signing key is stored in appsettings.json. This is intentionally demo-only. Mitigations: documented in README, recommended to use a proper user store, hashed passwords, and secrets management for production.
- Package vulnerabilities: During development Swashbuckle/Microsoft.OpenApi packages caused runtime issues and package advisories were flagged; I removed Swagger to keep the demo runnable and noted the advisories. For production, use up-to-date, audited packages.
- Input validation: Added basic validation in services and controllers to prevent invalid data (ISRC uniqueness, required fields). For production, use FluentValidation and more granular error handling.

3) One thing the AI got wrong and you fixed
- The initial code attempted to add Swagger/OpenAPI packages that conflicted with installed Microsoft.OpenApi version, causing TypeLoadExceptions at runtime. I removed the Swagger registration to make the app runnable and documented the issue. I also added static file serving so the ClientApp is accessible.

Additional notes on decisions
- Front-end: I implemented a minimal React SPA (ClientApp) that meets Task 2 requirements: a Track List view with status filter and a Track Detail view showing DSP distributions. It uses fetch and the demo JWT for the protected distribute call.
- Migrations: For speed and simplicity the app uses EnsureCreated() and HasData seeding. I documented how to add EF Core migrations in README; I can add migration files and switch to migrations on request.
- Commits & repo: I preserved incremental commits in the repository. If you want a different commit breakdown or a dedicated feature-branch workflow, I can prepare and push it.

