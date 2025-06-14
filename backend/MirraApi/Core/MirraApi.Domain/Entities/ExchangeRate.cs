namespace MirraApi.Domain.Entities;

public class ExchangeRate
{
    public int Id { get; set; }
    public double Rate { get; set; }
    public DateTime UpdatedAt { get; set; }
}