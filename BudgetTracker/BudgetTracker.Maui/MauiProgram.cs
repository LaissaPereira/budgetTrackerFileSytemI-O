using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BudgetTracker.Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddSingleton(_ =>
		{
			var filePath = Path.Combine(FileSystem.AppDataDirectory, "transactions.json");
			return new TransactionStorage(filePath);
		});
		builder.Services.AddSingleton<TransactionService>();
		builder.Services.AddSingleton(sp =>
		{
			var logFilePath = Path.Combine(FileSystem.AppDataDirectory, "logs", "transaction.log");
			return new LoggerService(sp.GetRequiredService<TransactionService>(), logFilePath);
		});
		
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<AddTransactionPage>();
		builder.Services.AddTransient<EditTransactionPage>();
		
		var app = builder.Build();
		app.Services.GetRequiredService<LoggerService>();
		return app;
	}
}
