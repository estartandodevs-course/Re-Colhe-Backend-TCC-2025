using Amazon.Lambda.AspNetCoreServer.Hosting;
using ReColhe.API.Infrastructure;
using ReColhe.ServiceDefaults;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add Lambda hosting
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // Use a specific MySQL version or get it from configuration
    // AutoDetect can fail during startup in serverless environments
    var serverVersion = new MySqlServerVersion(new Version(8, 0, 21)); // Adjust to your MySQL version
    options.UseMySql(connectionString, serverVersion, options =>
    {
        options.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    });
});

// Add Controllers
builder.Services.AddControllers();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "ReColhe API",
        Description = "AWS Lambda ASP.NET Core API ReColhe",
    });
    
    // Include XML comments
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add Exception Handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();
// Apply pending migrations at startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    try
    {
        if (db.Database.GetPendingMigrations().Any())
        {
            logger.LogInformation("Applying pending migrations...");
            db.Database.Migrate();
            logger.LogInformation("Migrations applied successfully.");
        }
        else
        {
            logger.LogInformation("No pending migrations. Ensuring database is created...");
            db.Database.EnsureCreated();
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error applying migrations or ensuring database is created. " +
                           "The application will continue but database operations may fail.");
        // Don't throw - allow the app to start even if migrations fail
        // This is important for Lambda cold starts
    }
}

// Configure the HTTP request pipeline
app.UseExceptionHandler();

// Configure Swagger (before MapDefaultEndpoints to avoid conflicts)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReColhe API V1");
        c.RoutePrefix = "swagger"; // Swagger UI at /swagger
        c.DocumentTitle = "ReColhe API Documentation";
        c.DefaultModelsExpandDepth(-1); // Hide schemas by default
        c.DisplayRequestDuration(); // Show request duration in Swagger UI
    });
}

app.MapDefaultEndpoints();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

// Map Controllers
app.MapControllers();

app.Run();

/// <summary>
/// Global exception handler for better error responses
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unhandled exception occurred");

        var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = "An error occurred while processing your request",
            Detail = exception.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status ?? 500;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
