using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Phonebook.API.Constants;
using Phonebook.API.Data;
using Phonebook.API.Helpers;
using Models = Phonebook.API.Models;

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
      if (!IsValidUserIdAgrument(context)) return false;

      return await IsUserValid(context);
    }


    private bool IsValidUserIdAgrument(ActionExecutingContext context)
    {
      if (context.HasId()) return true;

      context.Result = new BadRequestObjectResult("Bad input parameter");
      return false;
    }

    private async Task<bool> IsUserValid(ActionExecutingContext context)
    {
      return await Task.Run(() =>
      {
        var userId = context.GetId();
        if (userId == -1) return false;

        var userClaim = TryGetJwtClaim(context);
        if (userClaim == userId.ToString()) return true;

        context.Result = new UnauthorizedResult();
        return false;
      });
    }
  }
}