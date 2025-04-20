using System.Security.Claims;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Application.Common.Models;
using Throw;

namespace GymManagement.Api.Services;

public class CurrentUserProvider(IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
{
    public CurrentUser GetCurrentUser()
    {
        httpContextAccessor.HttpContext.ThrowIfNull();

        var id = GetClaimValues("id")
            .Select(Guid.Parse)
            .First();
        
        var permissions = GetClaimValues("permissions");
        
        var roles = GetClaimValues(ClaimTypes.Role);
        
        //var roleClaim = httpContextAccessor.HttpContext.User.Claims.First(claim => claim.Type == "role");

        return new CurrentUser(
            Id: id, 
            Permissions: permissions,
            Roles: roles);
    }

    private IReadOnlyList<string> GetClaimValues(string claimType)
    {
        return httpContextAccessor.HttpContext.User.Claims
            .Where(claim => claim.Type == claimType)
            .Select(claim => claim.Value)
            .ToList();
    }
}