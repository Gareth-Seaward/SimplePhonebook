using System.Collections.Generic;
using System.Security.Claims;
using Moq;

namespace phonebook.API.Tests.ActionFilters
{
  public abstract class MockClaimPrinciple
  {
    protected Mock<System.Security.Claims.ClaimsPrincipal> GetClaimsPrincipleMock()
    {
      var testClaim = CreateClaim();

      var mockClaimsPrinciple = new Mock<ClaimsPrincipal>();
      mockClaimsPrinciple
        .SetupGet(mcp => mcp.Claims)
        .Returns(new List<Claim> { testClaim });

      return mockClaimsPrinciple;
    }

    private Claim CreateClaim()
    {
      return new Claim(ClaimTypes.NameIdentifier, "1");
    }
  }
}