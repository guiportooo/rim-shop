namespace Shop.Api.Shared.Core.Events;

public record OrderCreated(int OrderId, IEnumerable<(Guid productCode, int quantity)> Items) : INotification;
