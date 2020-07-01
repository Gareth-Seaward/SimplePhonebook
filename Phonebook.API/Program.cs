using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Phonebook.API.Data;

namespace Phonebook.API
{
  public class Program
  {
    public static void Main(string[] args)
    {
      DoHostRun(args);
    }

    private static void DoHostRun(string[] args)
    {
      var host = CreateHostBuilder(args).Build();

      using (var scope = host.Services.CreateScope())
      {
        var services = scope.ServiceProvider;

        try
        {
          var context = services.GetRequiredService<DataContext>();
          context.Database.Migrate();
          Seed.SeedUsers(context);
          Seed.SeedEntries(context);
        }
        catch (Exception ex)
        {
          var logger = services.GetRequiredService<ILogger<Program>>();
          logger.LogError(ex, "An error occured during migration");
        }

      }

      host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
       Host.CreateDefaultBuilder(args)
       .ConfigureLogging(logging =>
        {
          logging.ClearProviders();
          logging.AddConsole();
        })
       .ConfigureWebHostDefaults(webBuilder =>
       {
         webBuilder.UseStartup<Startup>();
       });

  }
}
