using System.Text.Json.Serialization;

namespace MirraApi.Domain.Entities;

public class Payment
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;

    public int ClientId { get; set; }
    [JsonIgnore]
    public Client Client { get; set; } = null!;
}