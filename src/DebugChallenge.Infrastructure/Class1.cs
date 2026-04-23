using DebugChallenge.Application.Abstractions;
using DebugChallenge.Domain.Entities;

namespace DebugChallenge.Infrastructure.Pricing;

public sealed class InMemoryProductRepository : IProductRepository
{
    private static readonly Dictionary<string, Product> Products = new(StringComparer.OrdinalIgnoreCase)
    {
        ["KB-01"] = new Product("KB-01", "Mechanical Keyboard", 79.99m),
        ["MS-02"] = new Product("MS-02", "Gaming Mouse", 49.50m),
        ["HD-03"] = new Product("HD-03", "External SSD 1TB", 129.00m)
    };

    public Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
    {
        Products.TryGetValue(sku.Trim().ToUpperInvariant(), out var product);
        return Task.FromResult(product);
    }
}

public sealed class FixedCouponPolicy : ICouponPolicy
{
    public decimal GetDiscountPercentage(string? couponCode)
    {
        if (string.IsNullOrWhiteSpace(couponCode))
            return 0m;

        return couponCode.Trim().ToUpperInvariant() switch
        {
            "SAVE10" => 10m,
            "SAVE20" => 20m,
            _ => 0m
        };
    }
}
