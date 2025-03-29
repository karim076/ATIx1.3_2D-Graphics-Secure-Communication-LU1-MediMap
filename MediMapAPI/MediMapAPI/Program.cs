using DataAccess.DbContext;
using DataAccess.Repository;
using DataAccess.Repository.iUnitOfWork;
using MediMapAPI.Service;
using MediMapAPI.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MediMapAPI.Models;
using Models;
using DataAccess.Repository.IRepository;
using MediMap.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

// Add logging for debugging
builder.Services.AddHttpLogging(o => { });

// Configure database connection
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// ?? throw new InvalidOperationException("Database connection string is missing!");

// Configure JWT settings
string jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("JWT Key is missing in the configuration!");

//  dependency injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<UserService>();
// Repositories
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(Repository<>)); // Ensure generic base repository is also registered

// services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();


// Configure TokenSettings
builder.Services.Configure<TokenSettings>(options =>
{
    options.Key = jwtKey;
    options.Audience = "MediMapAPI"; // Set audience
    options.Issuer = "MediMapAPI"; // Set issuer
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(option =>
{
    option.SignIn.RequireConfirmedAccount = false;
    option.SignIn.RequireConfirmedPhoneNumber = false;
    option.SignIn.RequireConfirmedEmail = false;

}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
// Add controllers
/*builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});*/

// Add JWT authentication
/*builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidAudience = "MediMapAPI",
        ValidIssuer = "MediMapAPI",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)) // Use the configured JWT key
    };
});
*/
// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add authorization
builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
/*    app.MapOpenApi();*/
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add a redirect from the root URL to Swagger UI
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger/index.html");
    return Task.CompletedTask;
});

// redirect HTTP to HTTPS
app.UseHttpsRedirection();
app.UseHsts();

// Add security headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("Referrer-Policy", "no-referrer");
    context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'");
    context.Response.Headers.TryAdd("Permissions-Policy", "geolocation=(), microphone=(), camera=()");
    await next();
});
// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpLogging();
app.MapControllers();
app.Run();
