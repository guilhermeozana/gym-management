using FluentValidation;
using GymManagement.Application.Common.Behaviors;
using GymManagement.Application.Gyms.Commands.CreateGym;
using GymManagement.Domain.Gyms;
using NSubstitute;
using ErrorOr;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using TestCommon.Gyms;
using TestCommon.Subscriptions;

namespace GymManagement.Application.UnitTests.Common.Behaviors;

public class ValidationBehaviorTests
{
    private readonly ValidationBehavior<CreateGymCommand, ErrorOr<Gym>> _validationBehavior;
    private readonly IValidator<CreateGymCommand> _mockValidator;
    private readonly RequestHandlerDelegate<ErrorOr<Gym>> _mockNextBehavior;
    
    public ValidationBehaviorTests()
    {
        // Create a next behavior (mock)
        _mockNextBehavior = Substitute.For<RequestHandlerDelegate<ErrorOr<Gym>>>();
        
        // Create validator (mock)
        _mockValidator = Substitute.For<IValidator<CreateGymCommand>>();
        
        // Create validation behavior (SUT)
        _validationBehavior = new ValidationBehavior<CreateGymCommand, ErrorOr<Gym>>(_mockValidator);
    }
    
    [Fact]
    public async Task InvokeBehavior_WhenValidatorResultIsValid_ShouldInvokeNextBehavior()
    {
        // Arrange
        var createGymRequest = GymCommandFactory.CreateCreateGymCommand();
        var gym = GymFactory.CreateGym();
        
        _mockNextBehavior.Invoke().Returns(gym);
        
        _mockValidator
            .ValidateAsync(createGymRequest, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());
        
        // Act
        var result = await _validationBehavior.Handle(createGymRequest, _mockNextBehavior, default);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(gym);
    }
    
    [Fact]
    public async Task InvokeBehavior_WhenValidatorResultIsNotValid_ShouldReturnLisOfErrors()
    {
        // Arrange
        var createGymRequest = GymCommandFactory.CreateCreateGymCommand();

        List<ValidationFailure> validationFailures = [new(propertyName: "foo", errorMessage: "bad foo")];
        
        _mockValidator
            .ValidateAsync(createGymRequest, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(validationFailures));
        
        // Act
        var result = await _validationBehavior.Handle(createGymRequest, _mockNextBehavior, default);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("foo");
        result.FirstError.Description.Should().Be("bad foo");
    }
}