namespace MirraApi.Models.RequestModels;

public record CreateClientRequest(string Name, string Email, decimal BalanceT);