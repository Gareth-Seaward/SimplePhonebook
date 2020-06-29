using Microsoft.AspNetCore.Builder;
using Phonebook.API.Middleware;

namespace Phonebook.API.Helpers
{
  public static class ExceptionMiddlewareExtensions
  {
    public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
    {
      app.UseMiddleware<ExceptionMiddleware>();
    }
  }
}