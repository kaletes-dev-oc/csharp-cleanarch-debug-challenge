using DebugChallenge.Domain.Entities;

namespace DebugChallenge.Application.Abstractions;

public interface IProductRepository
{
    Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);
}
