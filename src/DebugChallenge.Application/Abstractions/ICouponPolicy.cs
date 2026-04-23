namespace DebugChallenge.Application.Abstractions;

public interface ICouponPolicy
{
    decimal GetDiscountPercentage(string? couponCode);
}
