using FluentAssertions;
using GymManagement.Application.SubcutaneousTests.Common;
using GymManagement.Application.Subscriptions.Commands.CreateSubscription;
using GymManagement.Domain.Subscriptions;
using MediatR;
using TestCommon.Gyms;

namespace GymManagement.Application.SubcutaneousTests.Gyms.Commands;

[Collection(MediatorFactoryCollection.CollectionName)]
public class CreateGymTests(MediatorFactory mediatorFactory)
{
    private readonly IMediator _mediator = mediatorFactory.CreateMediator();
    
    public async void CreateGym_WhenValidCommand_ShouldCreateGym()
    {
        // Arrange
        // Create a subscription
        var subscription = await CreateSubscription();

        // Create a valid CreateGymCommand
        var createGymCommand = GymCommandFactory.CreateCreateGymCommand(subscriptionId: subscription.Id);

        // Act
        // Send the CreateGymCommand to MediatR 
        var createGymResult = await _mediator.Send(createGymCommand);

        // Assert
        // The result is a gym corresponding to the details in the create gym command
        createGymResult.IsError.Should().BeFalse();
        createGymResult.Value.Should().NotBeNull();
        createGymResult.Value.SubscriptionId.Should().Be(subscription.Id);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(200)]
    public async Task CreateGym_WhenCommandContainsInvalidData_ShouldReturnValidationError(int gymNameLength)
    {
        // Arrange
        string gymName = new('a', gymNameLength);
        var createGymCommand = GymCommandFactory.CreateCreateGymCommand(name: gymName);

        // Act
        var result = await _mediator.Send(createGymCommand);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Name");
    }

    private async Task<Subscription> CreateSubscription()
    {
        //  1. Create a CreateSubscriptionCommand
        var createSubscriptionCommand = SubscriptionCommandFactory.CreateCreateSubscriptionCommand();
        
        //  2. Sending it to MediatR
        var result = await _mediator.Send(createSubscriptionCommand);
        
        //  3. Making sure it was created successfully
        result.IsError.Should().BeFalse();

        return result.Value;
    }
}