using ErrorOr;
using MediatR;
using GymManagement.Domain.Subscriptions;

namespace GymManagement.Application.Subscriptions.Queries;

public record GetSubscriptionQuery(Guid SubscriptionId) 
    : IRequest<ErrorOr<Subscription>>;