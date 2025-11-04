using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReColhe.API.Infrastructure;

namespace ReColhe.API.Controllers;

/// <summary>
/// Controller for health checks and database connection verification
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class HealthController : ControllerBase
{
    private readonly ILogger<HealthController> _logger;
    private readonly ReColheDbContext _context;

    public HealthController(ILogger<HealthController> logger, ReColheDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    /// <summary>
    /// Verifica se a conexão com o banco de dados está funcionando
    /// </summary>
    [HttpGet("database")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult> CheckDatabaseConnection(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Verificando conexão com o banco de dados");
            
            // Tenta fazer uma query simples para verificar a conexão
            var canConnect = await _context.Database.CanConnectAsync(cancellationToken);
            
            if (canConnect)
            {
                // Faz uma query simples para garantir que a conexão está realmente funcionando
                await _context.Database.ExecuteSqlRawAsync("SELECT 1", cancellationToken);
                
                var serverVersion = await _context.Database.GetServerVersionAsync(cancellationToken);
                
                _logger.LogInformation("Conexão com o banco de dados verificada com sucesso. Versão do servidor: {ServerVersion}", serverVersion);
                
                return Ok(new
                {
                    status = "ok",
                    message = "Conexão com o banco de dados está funcionando corretamente",
                    serverVersion = serverVersion?.ToString(),
                    timestamp = DateTime.UtcNow
                });
            }
            else
            {
                _logger.LogWarning("Não foi possível conectar ao banco de dados");
                
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new
                {
                    status = "error",
                    message = "Não foi possível conectar ao banco de dados",
                    timestamp = DateTime.UtcNow
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao verificar conexão com o banco de dados");
            
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new
            {
                status = "error",
                message = "Erro ao verificar conexão com o banco de dados",
                error = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }
}

