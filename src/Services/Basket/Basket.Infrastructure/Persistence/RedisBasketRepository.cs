using System.Text.Json;
using Basket.Application.Contracts;
using Basket.Domain.Entities;
using StackExchange.Redis;

namespace Basket.Infrastructure.Persistence;

public sealed class RedisBasketRepository : IBasketRepository
{
    private static readonly TimeSpan BasketLifetime =
        TimeSpan.FromDays(30);

    private readonly IDatabase _database;

    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web);

    public RedisBasketRepository(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task<ShoppingBasket?> GetAsync(
        Guid basketId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var value = await _database.StringGetAsync(GetKey(basketId));

        if (value.IsNullOrEmpty)
            return null;

        return JsonSerializer.Deserialize<ShoppingBasket>(
            value.ToString(), JsonOptions);
    }

    public async Task SaveAsync(
        ShoppingBasket basket,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var json = JsonSerializer.Serialize(basket, JsonOptions);

        await _database.StringSetAsync(
            GetKey(basket.Id),
            json,
            BasketLifetime);
    }

    public async Task DeleteAsync(
        Guid basketId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await _database.KeyDeleteAsync(GetKey(basketId));
    }

    private static string GetKey(Guid basketId)
        => $"basket:{basketId}";
}