using System;

namespace GymManagement.Contracts.Subscriptions;

public record SubscriptionResponse(Guid Id, SubscriptionType SubscriptionType);