using GymManagement.Application.Common.Interfaces;
using GymManagement.Application.Common.Models;
using Throw;

namespace GymManagement.Api.Services;

public class CurrentUserProvider(IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
{
    public CurrentUser GetCurrentUser()
    {
        httpContextAccessor.HttpContext.ThrowIfNull();

        var idClaim = httpContextAccessor.HttpContext.User.Claims.First(claim => claim.Type == "id");
        var permissionClaim = httpContextAccessor.HttpContext.User.Claims
            .Where(claim => claim.Type == "permissions")
            .Select(claim => claim.Value)
            .ToList();
        //var roleClaim = httpContextAccessor.HttpContext.User.Claims.First(claim => claim.Type == "role");

        return new CurrentUser(
            Id: Guid.Parse(idClaim.Value), 
            Permissions: permissionClaim);
    }
}