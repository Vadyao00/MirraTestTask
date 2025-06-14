using Contracts.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MirraApi.Domain.Entities;
using MirraApi.Models.RequestModels;

namespace Mirra.API.Controllers;

[ApiController]
[Route("rate")]
public class ExchangeRatesController(IRepositoryManager repositoryManager) : ControllerBase
{
    private readonly IRepositoryManager _repositoryManager = repositoryManager;

    [HttpGet]
    public async Task<IActionResult> GetLatestRate()
    {
        var latestRateEntity = await _repositoryManager.ExchangeRate.GetLatestRateAsync();
        
        var rateValue = latestRateEntity?.Rate ?? 10.0;
        
        return Ok(new { rate = rateValue });
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UpdateRate([FromBody] UpdateRateRequest request)
    {
        var newRate = new ExchangeRate
        {
            Rate = request.Rate,
            UpdatedAt = DateTime.UtcNow
        };
        
        _repositoryManager.ExchangeRate.CreateRate(newRate);
        
        await _repositoryManager.SaveAsync();
        
        return Ok(new { rate = newRate.Rate });
    }
}