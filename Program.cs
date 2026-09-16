
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Takwene.Application.Interfaces;
using Takwene.Application.Services;
using Takwene.Infrastructure.Persistence;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var configuration = builder.Configuration;

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(configuration.GetConnectionString("Default") ?? "Data Source=takwene.db"));

// Application services
builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<ITrackService, TrackService>();

// Authentication (JWT)
var jwtKey = configuration["Jwt:Key"] ?? "SuperSecretDemoKeyChangeMe";
var key = Encoding.ASCII.GetBytes(jwtKey);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddControllers();

var app = builder.Build();

// Ensure DB created and migrations applied at startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// Serve ClientApp static files if present
var clientAppPath = Path.Combine(app.Environment.ContentRootPath, "ClientApp");
if (Directory.Exists(clientAppPath))
{
    app.UseDefaultFiles(new DefaultFilesOptions
    {
        DefaultFileNames = { "index.html" }
    });
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(clientAppPath),
        RequestPath = ""
    });
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
