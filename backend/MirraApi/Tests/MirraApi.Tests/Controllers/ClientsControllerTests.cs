using Contracts.IRepositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Mirra.API.Controllers;
using MirraApi.Domain.Entities;
using MirraApi.Models.RequestModels;
using Moq;

namespace MirraApi.Tests.Controllers;

public class ClientsControllerTests
{
    private readonly Mock<IRepositoryManager> _repositoryManagerMock;
    private readonly Mock<IClientRepository> _clientRepositoryMock;
    private readonly ClientsController _controller;

    public ClientsControllerTests()
    {
        _repositoryManagerMock = new Mock<IRepositoryManager>();
        _clientRepositoryMock = new Mock<IClientRepository>();
        
        _repositoryManagerMock.Setup(rm => rm.Client).Returns(_clientRepositoryMock.Object);
        _controller = new ClientsController(_repositoryManagerMock.Object);
    }

    [Fact]
    public async Task GetAllClients_ReturnsOkResult_WithClients()
    {
        var clients = new List<Client>
        {
            new() { Id = 1, Name = "Test Client 1", Email = "client1@test.com", BalanceT = 100 },
            new() { Id = 2, Name = "Test Client 2", Email = "client2@test.com", BalanceT = 200 }
        };

        _clientRepositoryMock.Setup(repo => repo.GetAllClientsAsync(false))
            .ReturnsAsync(clients);

        var result = await _controller.GetAllClients();

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedClients = okResult.Value.Should().BeAssignableTo<IEnumerable<Client>>().Subject;
        returnedClients.Should().HaveCount(2);
        returnedClients.Should().BeEquivalentTo(clients);
    }

    [Fact]
    public async Task GetClientById_WithValidId_ReturnsOkResult()
    {
        var client = new Client { Id = 1, Name = "Test Client", Email = "test@test.com", BalanceT = 100 };
        _clientRepositoryMock.Setup(repo => repo.GetClientByIdAsync(1, false))
            .ReturnsAsync(client);

        var result = await _controller.GetClientById(1);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedClient = okResult.Value.Should().BeOfType<Client>().Subject;
        returnedClient.Should().BeEquivalentTo(client);
    }

    [Fact]
    public async Task GetClientById_WithInvalidId_ReturnsNotFound()
    {
        _clientRepositoryMock.Setup(repo => repo.GetClientByIdAsync(999, false))
            .ReturnsAsync((Client)null);

        var result = await _controller.GetClientById(999);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task CreateClient_WithValidData_ReturnsCreatedResult()
    {
        var createRequest = new CreateClientRequest("New Client", "new@test.com", 100);

        var createdClient = new Client
        {
            Id = 1,
            Name = createRequest.Name,
            Email = createRequest.Email,
            BalanceT = createRequest.BalanceT
        };

        _clientRepositoryMock.Setup(repo => repo.CreateClient(It.IsAny<Client>()))
            .Callback<Client>(client => client.Id = 1);
        
        _repositoryManagerMock.Setup(rm => rm.SaveAsync())
            .Returns(Task.CompletedTask);

        var result = await _controller.CreateClient(createRequest);

        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.ActionName.Should().Be(nameof(_controller.GetClientById));
        createdResult.RouteValues["id"].Should().Be(1);
        var returnedClient = createdResult.Value.Should().BeOfType<Client>().Subject;
        returnedClient.Id.Should().Be(1);
        returnedClient.Name.Should().Be(createRequest.Name);
        returnedClient.Email.Should().Be(createRequest.Email);
        returnedClient.BalanceT.Should().Be(createRequest.BalanceT);
    }
} 