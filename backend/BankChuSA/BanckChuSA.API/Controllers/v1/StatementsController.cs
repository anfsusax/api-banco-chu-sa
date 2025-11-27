using BankChuSA.Application.DTOs;
using BankChuSA.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankChuSA.API.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class StatementsController : ControllerBase
{
    private readonly IStatementService _statementService;

    public StatementsController(IStatementService statementService)
    {
        _statementService = statementService;
    }

    [HttpGet("{accountNumber}")]
    [ProducesResponseType(typeof(StatementDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StatementDto>> GetStatement(
        string accountNumber,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        try
        {
            var statement = await _statementService.GetStatementAsync(accountNumber, startDate, endDate);
            return Ok(statement);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}

