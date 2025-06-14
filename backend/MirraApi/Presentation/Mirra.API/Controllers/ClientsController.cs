using Contracts.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MirraApi.Domain.Entities;
using MirraApi.Models.RequestModels;

namespace Mirra.API.Controllers;

[ApiController]
[Route("clients")] 
[Authorize]
public class ClientsController(IRepositoryManager repositoryManager) : ControllerBase
{
    private readonly IRepositoryManager _repositoryManager = repositoryManager;

    [HttpGet]
    public async Task<IActionResult> GetAllClients()
    {
        var clients = await _repositoryManager.Client.GetAllClientsAsync(trackChanges: false);
        return Ok(clients);
    }
    
    [HttpGet("{id:int}", Name = "GetClientById")]
    public async Task<IActionResult> GetClientById(int id)
    {
        var client = await _repositoryManager.Client.GetClientByIdAsync(id, trackChanges: false);
        if (client == null)
        {
            return NotFound();
        }
        return Ok(client);
    }

    [HttpPost]
    public async Task<IActionResult> CreateClient([FromBody] CreateClientRequest request)
    {
        var client = new Client
        {
            Name = request.Name,
            Email = request.Email,
            BalanceT = request.BalanceT
        };
        
        _repositoryManager.Client.CreateClient(client);
        await _repositoryManager.SaveAsync();
        
        return CreatedAtAction(nameof(GetClientById), new { id = client.Id }, client);
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateClient(int id, [FromBody] UpdateClientRequest request)
    {
        var client = await _repositoryManager.Client.GetClientByIdAsync(id, trackChanges: true);
        if (client == null)
        {
            return NotFound();
        }

        client.Name = request.Name;
        client.Email = request.Email;
        client.BalanceT = request.BalanceT;

        await _repositoryManager.SaveAsync();
        
        return Ok(client);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        var client = await _repositoryManager.Client.GetClientByIdAsync(id, trackChanges: true);
        if (client == null)
        {
            return NotFound();
        }
        
        _repositoryManager.Client.DeleteClient(client);
        await _repositoryManager.SaveAsync();
        
        return NoContent();
    }
}