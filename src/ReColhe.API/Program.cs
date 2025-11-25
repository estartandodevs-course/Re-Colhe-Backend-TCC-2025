using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using ReColhe.API.Infrastructure;
using ReColhe.API.Infrastructure.Repository;
using ReColhe.Application.Usuarios.Criar;
using ReColhe.Domain.Repository;
using ReColhe.ServiceDefaults;
using ReColhe.Application.Reclamacoes.Listar;

using ReColhe.Application.Common.Interfaces;
using ReColhe.API.Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ReColhe.Application.Auth.Login;
using ReColhe.Application.Empresas.Criar;
using ReColhe.Application.Empresas.Listar;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add Lambda hosting
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

builder.Services.AddScoped<IEmpresaRepository, EmpresaRepository>();

builder.Services.AddScoped<IPevRepository, PevRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();


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
builder.Services.AddScoped<IReclamacaoRepository, ReclamacaoRepository>();
builder.Services.AddScoped<INotificacaoRepository, NotificacaoRepository>();
builder.Services.AddScoped<IApoioReclamacaoRepository, ApoioReclamacaoRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IUsuarioNotificacaoRepository, UsuarioNotificacaoRepository>();
builder.Services.AddScoped<IUsuarioPevFavoritoRepository, UsuarioPevFavoritoRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();

// Add Controllers
builder.Services.AddControllers();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(CriarUsuarioCommand).Assembly,
    typeof(ListarReclamacoesQuery).Assembly,
    typeof(LoginCommand).Assembly,
    typeof(CriarEmpresaCommand).Assembly,
    typeof(ListarEmpresasQuery).Assembly
));

var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

if (string.IsNullOrEmpty(jwtKey) || jwtKey.Length < 32)
{
    throw new InvalidOperationException("A configuração 'Jwt:Key' é inválida. Ela deve existir e ter pelo menos 32 caracteres para ser segura.");
}

// Add Swagger/OpenAPI
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});
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
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    var jsonPath = builder.Configuration["Swagger:JsonPath"];

    c.SwaggerEndpoint(jsonPath, "ReColhe API V1");
    c.RoutePrefix = "api/swagger";
    c.DocumentTitle = "ReColhe API Documentation";
    c.DefaultModelsExpandDepth(-1);
    c.DisplayRequestDuration();
});

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
app.UseAuthentication();
app.UseAuthorization();
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