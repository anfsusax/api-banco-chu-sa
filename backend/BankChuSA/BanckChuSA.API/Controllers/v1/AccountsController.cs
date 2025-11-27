using BankChuSA.Application.DTOs;
using BankChuSA.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankChuSA.API.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AccountDto>> CreateAccount([FromBody] CreateAccountDto createAccountDto)
    {
        var account = await _accountService.CreateAccountAsync(createAccountDto);
        return CreatedAtAction(nameof(GetAccount), new { accountNumber = account.AccountNumber }, account);
    }

    [HttpGet("{accountNumber}")]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AccountDto>> GetAccount(string accountNumber)
    {
        var account = await _accountService.GetAccountByNumberAsync(accountNumber);
        
        if (account == null)
            return NotFound();

        return Ok(account);
    }
}

