namespace DebugChallenge.Application.UseCases.CalculateCartTotal;

public sealed record CalculateCartTotalCommand(
    IReadOnlyCollection<CartLineInput> Lines,
    string? CouponCode = null);
