using Contracts.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mirra.API.Controllers;

[ApiController]
[Route("payments")]
[Authorize]
public class PaymentsController(IRepositoryManager repositoryManager) : ControllerBase
{
    private readonly IRepositoryManager _repositoryManager = repositoryManager;
    
    [HttpGet]
    public async Task<IActionResult> GetRecentPayments([FromQuery] int take = 5)
    {
        if (take <= 0)
        {
            return BadRequest("Параметр 'take' должен быть положительным числом.");
        }

        var payments = await _repositoryManager.Payment.GetRecentPaymentsAsync(take);
        
        return Ok(payments);
    }

    [HttpGet("client/{clientId}")]
    public async Task<IActionResult> GetClientPayments(int clientId)
    {
        var client = await _repositoryManager.Client.GetClientByIdAsync(clientId, false);
        if (client == null)
        {
            return NotFound($"Клиент с ID {clientId} не найден.");
        }

        var payments = await _repositoryManager.Payment.GetClientPaymentsAsync(clientId);
        return Ok(payments);
    }
}