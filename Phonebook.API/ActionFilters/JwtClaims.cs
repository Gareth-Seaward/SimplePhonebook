using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Phonebook.API.Helpers;

namespace Phonebook.API.ActionFilters
{
    public abstract class JwtClaims
    {
        public string TryGetJwtClaim(ActionExecutingContext context)
        {
          try
          {
              return GetJwtClaim(context);
          }
          catch (System.Exception)
          {
              context.Result = new UnauthorizedResult();
              return string.Empty;
          }
        }

        private string GetJwtClaim(ActionExecutingContext context)
        {
          return context.HttpContext.User.GetUserIdClaim();
        }
    }
}