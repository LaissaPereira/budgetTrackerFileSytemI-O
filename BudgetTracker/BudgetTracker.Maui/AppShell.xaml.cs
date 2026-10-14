namespace BudgetTracker.Maui;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(AddTransactionPage), typeof(AddTransactionPage));
		Routing.RegisterRoute(nameof(EditTransactionPage), typeof(EditTransactionPage));
	}
}
