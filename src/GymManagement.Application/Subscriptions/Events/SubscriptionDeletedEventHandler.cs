using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Admins.Events;
using MediatR;

namespace GymManagement.Application.Subscriptions.Events;

public class SubscriptionDeletedEventHandler(
    ISubscriptionsRepository subscriptionsRepository,
    IUnitOfWork unitOfWork)
    : INotificationHandler<SubscriptionDeletedEvent>
{

    public async Task Handle(SubscriptionDeletedEvent notification, CancellationToken cancellationToken)
    {
        var subscription = await subscriptionsRepository.GetByIdAsync(notification.SubscriptionId);

        if (subscription is null)
        {
            throw new InvalidOperationException("Subscription not found");
        }
        
        await subscriptionsRepository.RemoveSubscriptionAsync(subscription);
        await unitOfWork.CommitChangesAsync();
    }
}