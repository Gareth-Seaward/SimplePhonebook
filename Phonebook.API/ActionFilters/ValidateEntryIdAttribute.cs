using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Phonebook.API.Constants;
using Phonebook.API.Data;
using Phonebook.API.Helpers;
using Models = Phonebook.API.Models;

namespace Phonebook.API.ActionFilters
{
  public class ValidateEntryIdAttribute : IAsyncActionFilter
  {
    private Models.Phonebook _phonebook;
    private readonly IEntriesRepository _entryRepo;

    public ValidateEntryIdAttribute(IEntriesRepository entryRepo)
    {
      _entryRepo = entryRepo;
    }
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      if (!await IsDoActionFilter(context)) return;

      await next();
    }

    private async Task<bool> IsDoActionFilter(ActionExecutingContext context)
    {
      if (!HasPhonebook(context)) return false;

      if(!HasValidEntryId(context)) return false;

      return await IsEntryBelongingToPhoneBook(context);
    }

    private bool HasPhonebook(ActionExecutingContext context)
    {
      if(!HasPhonebookKey(context))return false;
      _phonebook = context.HttpContext.Items[HttpContextConstants.PhonebookItem] as Models.Phonebook;
      return !(_phonebook is null);
    }

    private bool HasPhonebookKey(ActionExecutingContext context)
    {
      return context.HttpContext.Items.ContainsKey(HttpContextConstants.PhonebookItem);
    }

    private bool HasValidEntryId(ActionExecutingContext context)
    {
      if(context.HasEntryId() && context.GetEntryId() > 0) return true;

      context.Result = new BadRequestObjectResult("Bad input parameter");
      return false;
    }

    private async Task<bool> IsEntryBelongingToPhoneBook(ActionExecutingContext context)
    {
      var entryId = context.GetEntryId();
      var entry = await _entryRepo.GetEntry(entryId);
      if (IsEntryPhonebookMatchingUserPhonebook(entry))
      {
        SaveEntryToContextCache(context, entry);
        return true;
      }

      context.Result = new UnauthorizedResult();
      return false;
    }

    private bool IsEntryPhonebookMatchingUserPhonebook(Models.Entry entry)
    {
      return entry.Phonebook.Id == _phonebook.Id;
    }

    private static void SaveEntryToContextCache(ActionExecutingContext context, Models.Entry entry)
    {
      context.HttpContext.Items[HttpContextConstants.EntryItem] = entry;
    }
  }
}