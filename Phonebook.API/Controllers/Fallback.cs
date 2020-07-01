using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace Phonebook.API.Controllers
{
  [ExcludeFromCodeCoverage] //Standard fallback pattern
  public class Fallback : Controller
  {
    public IActionResult Index()
    {
      return PhysicalFile(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"), "text/HTML");
    }
  }
}