namespace DebugChallenge.Application.UseCases.CalculateCartTotal;

public sealed record CalculateCartTotalResult(decimal Subtotal, decimal DiscountAmount, decimal Total);
