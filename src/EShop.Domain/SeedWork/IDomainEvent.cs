using MediatR;

namespace EShop.Domain.SeedWork;
public interface IDomainEvent : INotification
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
    string EventType { get; }
}
