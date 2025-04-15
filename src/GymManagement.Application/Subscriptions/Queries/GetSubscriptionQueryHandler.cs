using ErrorOr;
using MediatR;
using GymManagement.Domain.Subscriptions;
using System.Threading;
using System.Threading.Tasks;
using GymManagement.Application.Common.Interfaces;

namespace GymManagement.Application.Subscriptions.Queries;

public class GetSubscriptionQueryHandler 
    : IRequestHandler<GetSubscriptionQuery, ErrorOr<Subscription>>
{
    private readonly ISubscriptionsRepository _subscriptionsRepository;

    public GetSubscriptionQueryHandler(ISubscriptionsRepository subscriptionsRepository)
    {
        _subscriptionsRepository = subscriptionsRepository;
    }

    public async Task<ErrorOr<Subscription>> Handle(GetSubscriptionQuery request, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionsRepository.GetByIdAsync(request.SubscriptionId);

        return subscription is null
            ? Error.NotFound("Subscription Not Found.")
            : subscription;
    }
}