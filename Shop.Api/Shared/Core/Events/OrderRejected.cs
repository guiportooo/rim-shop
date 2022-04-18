namespace Shop.Api.Shared.Core.Events;

public record OrderRejected(int OrderId) : INotification;