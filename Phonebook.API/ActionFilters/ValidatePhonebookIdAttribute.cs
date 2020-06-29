using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Phonebook.API.Constants;
using Phonebook.API.Data;
using Phonebook.API.Helpers;

namespace Phonebook.API.ActionFilters
{
  public class ValidatePhonebookIdAttribute : JwtClaims, IAsyncActionFilter
  {
    private readonly IPhonebookRepository _phonebookRepo;

    public ValidatePhonebookIdAttribute(IPhonebookRepository phonebookRepo)
    {
      _phonebookRepo = phonebookRepo;
    }
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      if (!await IsDoActionFilter(context)) return;

      await next();
    }

    private async Task<bool> IsDoActionFilter(ActionExecutingContext context)
    {
      if (!HasPhonebookId(context)) return false;

      return await IsValidPhoneBook(context);
    }

    private bool HasPhonebookId(ActionExecutingContext context)
    {
      if (context.HasId()) return true;

      context.Result = new BadRequestObjectResult("Bad input parameter");
      return false;
    }

    private async Task<bool> IsValidPhoneBook(ActionExecutingContext context)
    {
      var phonebook = await _phonebookRepo.GetPhonebook(context.GetId());

      var userClaim = TryGetJwtClaim(context);
      if (IsPhonebookUserMatchingClaim(phonebook, userClaim))
      {
        SavePhonebookToContextCache(context, phonebook);
        return true;
      }

      context.Result = new UnauthorizedResult();
      return false;
      
    }

    private static void SavePhonebookToContextCache(ActionExecutingContext context, Models.Phonebook phonebook)
    {
      context.HttpContext.Items[HttpContextConstants.PhonebookItem] = phonebook;
    }

    private static bool IsPhonebookUserMatchingClaim(Models.Phonebook phonebook, string userClaim)
    {
      return phonebook.User.Id.ToString() == userClaim;
    }
  }
}