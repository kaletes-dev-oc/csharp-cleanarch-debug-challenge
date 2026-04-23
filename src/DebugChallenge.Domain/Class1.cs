namespace DebugChallenge.Domain.Entities;

public sealed class Product
{
    public Product(string sku, string name, decimal unitPrice)
    {
        if (string.IsNullOrWhiteSpace(sku))
            throw new ArgumentException("SKU is required", nameof(sku));

        if (unitPrice < 0)
            throw new ArgumentOutOfRangeException(nameof(unitPrice), "Unit price cannot be negative");

        Sku = sku.Trim().ToUpperInvariant();
        Name = name;
        UnitPrice = unitPrice;
    }

    public string Sku { get; }
    public string Name { get; }
    public decimal UnitPrice { get; }
}
