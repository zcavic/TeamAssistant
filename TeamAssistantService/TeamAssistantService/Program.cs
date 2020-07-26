using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace TeamAssistantService
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
				.Enrich.FromLogContext()
				.WriteTo.File(@"C:\temp\TeamAssistant\LogFile.txt")
				.CreateLogger();
			try
			{
				Log.Information("Starting up the service.");
				CreateHostBuilder(args).Build().Run();
				return;
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "There was a problem starting the services.");
				return;
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureServices((hostContext, services) =>
				{
					services.AddHostedService<Worker>();
				})
			.UseSerilog();
	}
}
