using Contracts.IRepositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Mirra.API.Controllers;
using MirraApi.Domain.Entities;
using MirraApi.Models.RequestModels;
using Moq;

namespace MirraApi.Tests.Controllers;

public class ExchangeRateControllerTests
{
    private readonly Mock<IRepositoryManager> _repositoryManagerMock;
    private readonly Mock<IExchangeRateRepository> _rateRepositoryMock;
    private readonly ExchangeRatesController _controller;

    public ExchangeRateControllerTests()
    {
        _repositoryManagerMock = new Mock<IRepositoryManager>();
        _rateRepositoryMock = new Mock<IExchangeRateRepository>();
        
        _repositoryManagerMock.Setup(rm => rm.ExchangeRate).Returns(_rateRepositoryMock.Object);
        _controller = new ExchangeRatesController(_repositoryManagerMock.Object);
    }

    [Fact]
    public async Task GetLatestRate_ReturnsOkResult()
    {
        var rate = new ExchangeRate { Id = 1, Rate = 10.5, UpdatedAt = DateTime.UtcNow };
        _rateRepositoryMock.Setup(repo => repo.GetLatestRateAsync())
            .ReturnsAsync(rate);

        var result = await _controller.GetLatestRate();

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedValue = okResult.Value;
        
        var rateProperty = returnedValue.GetType().GetProperty("rate");
        rateProperty.Should().NotBeNull();
        var rateValue = rateProperty.GetValue(returnedValue);
        ((double)rateValue).Should().Be(rate.Rate);
    }

    [Fact]
    public async Task GetLatestRate_WhenNoRate_ReturnsDefaultRate()
    {
        _rateRepositoryMock.Setup(repo => repo.GetLatestRateAsync())
            .ReturnsAsync((ExchangeRate)null);

        var result = await _controller.GetLatestRate();

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedValue = okResult.Value;
        
        var rateProperty = returnedValue.GetType().GetProperty("rate");
        rateProperty.Should().NotBeNull();
        var rateValue = rateProperty.GetValue(returnedValue);
        ((double)rateValue).Should().Be(10.0);
    }

    [Fact]
    public async Task UpdateRate_WithValidRate_ReturnsOkResult()
    {
        double newRate = 12.5;
        var updateRequest = new UpdateRateRequest(newRate);

        _rateRepositoryMock.Setup(repo => repo.CreateRate(It.IsAny<ExchangeRate>()))
            .Callback<ExchangeRate>(rate => rate.Id = 1);
            
        _repositoryManagerMock.Setup(rm => rm.SaveAsync())
            .Returns(Task.CompletedTask);

        var result = await _controller.UpdateRate(updateRequest);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedValue = okResult.Value;
        
        var rateProperty = returnedValue.GetType().GetProperty("rate");
        rateProperty.Should().NotBeNull();
        var rateValue = rateProperty.GetValue(returnedValue);
        ((double)rateValue).Should().Be(newRate);
        
        _rateRepositoryMock.Verify(repo => repo.CreateRate(It.Is<ExchangeRate>(r => r.Rate == newRate)), Times.Once);
        _repositoryManagerMock.Verify(rm => rm.SaveAsync(), Times.Once);
    }
} 