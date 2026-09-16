DECISIONS

1) What did AI generate and what was hand-written?
- AI-assisted: I used the assistant to scaffold the project structure, generate domain entities, DbContext seed code, and example controllers and services. I adapted and edited the generated code to ensure consistent namespaces and wiring.
- Hand-written: Manual adjustments included Program.cs wiring for static files and JWT setup, small validation logic in services, and the simple React client (CDN-based) to keep the demo lightweight.

2) Security issues found or introduced and mitigations
- Demo JWT implementation accepts any username/password and signs tokens with a symmetric key in appsettings.json. This is insecure for production; the README documents the demo nature and instructs to replace the key and add proper user authentication.
- The AuthController issues tokens without verifying credentials; mitigate by connecting to a user store and hashing passwords before issuing tokens.
- Ensure secrets (Jwt:Key) are stored in environment variables or a secrets manager in real deployments.

3) One thing AI got wrong and was fixed
- Initial project scaffolding lacked static file serving for the ClientApp. I added app.UseStaticFiles and default files configuration so the SPA is accessible at the root.


Notes:
- The project is a pragmatic, demo-focused subset of Clean Architecture: Domain, Application, Infrastructure folders are present and kept simple.
- For production, add proper migrations, tests, and secure authentication.

