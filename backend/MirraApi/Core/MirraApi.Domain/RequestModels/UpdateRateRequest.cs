using System.ComponentModel.DataAnnotations;

namespace MirraApi.Models.RequestModels;

public record UpdateRateRequest([Range(0.01, 1000)] double Rate);