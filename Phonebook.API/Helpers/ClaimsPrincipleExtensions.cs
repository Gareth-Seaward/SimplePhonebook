using System.Linq;
using System.Security.Claims;
using Phonebook.API.Exceptions;

namespace Phonebook.API.Helpers
{
  public static class ClaimsPrincipleExtensions
  {
    public static string GetUserIdClaim(this ClaimsPrincipal claimsPrincipal)
    {
      var claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

      if (claim is null)
        throw new JwtClaimNotFoundException(ClaimTypes.NameIdentifier);

      return claim.Value;
    }
  }
}