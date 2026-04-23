using DebugChallenge.Application.Abstractions;
using DebugChallenge.Application.UseCases.CalculateCartTotal;
using DebugChallenge.Domain.Entities;

namespace DebugChallenge.Application.Tests;

public class CalculateCartTotalHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Calculate_Total_For_Distinct_Items()
    {
        // Arrange
        var handler = new CalculateCartTotalHandler(new FakeProductRepository(), new FakeCouponPolicy());
        var command = new CalculateCartTotalCommand(new[]
        {
            new CartLineInput("KB-01", 1),
            new CartLineInput("MS-02", 2)
        });

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.Equal(178.99m, result.Subtotal);
        Assert.Equal(0m, result.DiscountAmount);
        Assert.Equal(178.99m, result.Total);
    }

    [Fact]
    public async Task HandleAsync_Should_Sum_Duplicate_SKUs_Before_Applying_Discount()
    {
        // Arrange
        var handler = new CalculateCartTotalHandler(new FakeProductRepository(), new FakeCouponPolicy());
        var command = new CalculateCartTotalCommand(new[]
        {
            new CartLineInput("KB-01", 1),
            new CartLineInput("KB-01", 2)
        }, "SAVE10");

        // Act
        var result = await handler.HandleAsync(command);

        // Expected subtotal: 3 * 79.99 = 239.97
        // Expected discount: 10% = 24.00
        // Expected total: 215.97
        Assert.Equal(239.97m, result.Subtotal);
        Assert.Equal(24.00m, result.DiscountAmount);
        Assert.Equal(215.97m, result.Total);
    }

    private sealed class FakeProductRepository : IProductRepository
    {
        private readonly Dictionary<string, Product> _data = new(StringComparer.OrdinalIgnoreCase)
        {
            ["KB-01"] = new Product("KB-01", "Mechanical Keyboard", 79.99m),
            ["MS-02"] = new Product("MS-02", "Gaming Mouse", 49.50m)
        };

        public Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
        {
            _data.TryGetValue(sku, out var product);
            return Task.FromResult(product);
        }
    }

    private sealed class FakeCouponPolicy : ICouponPolicy
    {
        public decimal GetDiscountPercentage(string? couponCode)
            => string.Equals(couponCode, "SAVE10", StringComparison.OrdinalIgnoreCase) ? 10m : 0m;
    }
}
