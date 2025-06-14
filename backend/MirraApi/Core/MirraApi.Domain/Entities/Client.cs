using System.Text.Json.Serialization;

namespace MirraApi.Domain.Entities;

public class Client
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal BalanceT { get; set; }

    [JsonIgnore]
    public List<Payment> Payments { get; set; } = [];
}