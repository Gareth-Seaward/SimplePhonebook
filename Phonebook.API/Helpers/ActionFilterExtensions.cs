using Microsoft.AspNetCore.Mvc.Filters;

namespace Phonebook.API.Helpers
{
  public static class ActionFilterExtensions
  {
    private const string IdKey = "id";
    public static bool HasId(this ActionExecutingContext context)
    {
      return context.ActionArguments.ContainsKey(IdKey);
    }

    public static int GetId(this ActionExecutingContext context)
    {
      if (int.TryParse(context.ActionArguments[IdKey].ToString(), out int id))
        return id;
      return -1;
    }
  }
}