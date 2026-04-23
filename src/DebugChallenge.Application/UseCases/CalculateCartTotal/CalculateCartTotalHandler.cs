using DebugChallenge.Application.Abstractions;

namespace DebugChallenge.Application.UseCases.CalculateCartTotal;

public sealed class CalculateCartTotalHandler
{
    private readonly IProductRepository _productRepository;
    private readonly ICouponPolicy _couponPolicy;

    public CalculateCartTotalHandler(IProductRepository productRepository, ICouponPolicy couponPolicy)
    {
        _productRepository = productRepository;
        _couponPolicy = couponPolicy;
    }

    public async Task<CalculateCartTotalResult> HandleAsync(
        CalculateCartTotalCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.Lines.Count == 0)
            throw new InvalidOperationException("Cart is empty.");

        // Intentional interview bug:
        // For example: KB-01:1,KB-01:2 should charge 3 units, but this logic charges only 1.
        var effectiveLines = command.Lines
            .Where(line => line.Quantity > 0)
            .DistinctBy(line => NormalizeSku(line.Sku))
            .ToList();

        decimal subtotal = 0m;

        foreach (var line in effectiveLines)
        {
            var sku = NormalizeSku(line.Sku);
            var product = await _productRepository.GetBySkuAsync(sku, cancellationToken);

            if (product is null)
                throw new InvalidOperationException($"Unknown SKU: {sku}");

            subtotal += product.UnitPrice * line.Quantity;
        }

        var discountPercentage = _couponPolicy.GetDiscountPercentage(command.CouponCode);
        var discountAmount = Math.Round(subtotal * (discountPercentage / 100m), 2, MidpointRounding.AwayFromZero);
        var total = subtotal - discountAmount;

        return new CalculateCartTotalResult(subtotal, discountAmount, total);
    }

    private static string NormalizeSku(string sku) => sku.Trim().ToUpperInvariant();
}
