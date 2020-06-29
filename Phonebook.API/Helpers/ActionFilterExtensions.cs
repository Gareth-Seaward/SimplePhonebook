using Microsoft.AspNetCore.Mvc.Filters;

namespace Phonebook.API.Helpers
{
  public static class ActionFilterExtensions
  {
    private const string IdKey = "id";
    private const string entryIdKey = "entryid";
    public static bool HasId(this ActionExecutingContext context)
    {
      return context.ActionArguments.ContainsKey(IdKey);
    }

    public static bool HasEntryId(this ActionExecutingContext context)
    {
      return context.ActionArguments.ContainsKey(entryIdKey);
    }

    public static int GetId(this ActionExecutingContext context)
    {
      if (int.TryParse(context.ActionArguments[IdKey].ToString(), out int id))
        return id;
      return -1;
    }

    public static int GetEntryId(this ActionExecutingContext context)
    {
      if (int.TryParse(context.ActionArguments[entryIdKey].ToString(), out int id))
        return id;
      return -1;
    }
  }
}