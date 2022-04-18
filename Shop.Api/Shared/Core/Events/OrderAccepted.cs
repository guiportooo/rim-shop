namespace Shop.Api.Shared.Core.Events;

public record OrderAccepted(int OrderId) : INotification;