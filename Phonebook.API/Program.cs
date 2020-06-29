using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
