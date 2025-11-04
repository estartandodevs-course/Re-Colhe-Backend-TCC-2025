using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReColhe.API.Infrastructure;

namespace ReColhe.API.Controllers;

/// <summary>
/// Controller for health check and database connection verification
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class HealthCheckController : ControllerBase
{
    private readonly ILogger<HealthCheckController> _logger;
    private readonly ApplicationDbContext _dbContext;

    public HealthCheckController(ILogger<HealthCheckController> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    /// <summary>
    /// Verifies database connection status
    /// </summary>
    /// <returns>Database connection status</returns>
    [HttpGet("database")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult> CheckDatabase(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Checking database connection...");
            
            // Try to connect to the database
            var canConnect = await _dbContext.Database.CanConnectAsync(cancellationToken);
            
            if (!canConnect)
            {
                _logger.LogWarning("Database connection failed");
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new
                {
                    status = "unhealthy",
                    database = "disconnected",
                    timestamp = DateTime.UtcNow,
                    message = "Unable to connect to the database"
                });
            }

                        // Execute a simple query to verify connectivity and get connection info
            await _dbContext.Database.ExecuteSqlRawAsync("SELECT 1", cancellationToken);                                                                        
            
            // Get connection info while connection is open
            var connection = _dbContext.Database.GetDbConnection();
            var connectionString = _dbContext.Database.GetConnectionString();
            string databaseName = connection.Database ?? "unknown";
            string? serverVersion = null;
            
            try
            {
                // Ensure connection is open to get server version
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    await connection.OpenAsync(cancellationToken);
                }
                serverVersion = connection.ServerVersion;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not retrieve server version");
            }

            _logger.LogInformation("Database connection successful");
            
            return Ok(new
            {
                status = "healthy",
                database = "connected",
                timestamp = DateTime.UtcNow,
                databaseName = databaseName,
                serverVersion = serverVersion ?? "unknown",                                                                            
                message = "Database connection is working correctly"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking database connection");
            
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new
            {
                status = "unhealthy",
                database = "error",
                timestamp = DateTime.UtcNow,
                message = ex.Message,
                error = ex.GetType().Name
            });
        }
    }

    /// <summary>
    /// General health check endpoint
    /// </summary>
    /// <returns>API health status</returns>
    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public ActionResult GetHealth()
    {
        return Ok(new
        {
            status = "healthy",
            timestamp = DateTime.UtcNow,
            message = "API is running"
        });
    }
}

