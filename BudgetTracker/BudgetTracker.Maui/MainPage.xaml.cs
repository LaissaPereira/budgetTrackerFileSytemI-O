using System.Globalization;
using System.Numerics;

namespace BudgetTracker.Maui;

public partial class MainPage : ContentPage
{
	private readonly TransactionService _transactionService;

	public MainPage(TransactionService transactionService)
	{
		InitializeComponent();
		_transactionService = transactionService;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		RefreshTransactions();
		ReportPanel.IsVisible = false;
    }

	private void RefreshTransactions()
	{
		TransactionsCollectionView.ItemsSource = _transactionService.GetTransactions();
	}

	private async void OnAddTransactionClicked(object? sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(AddTransactionPage));
	}

	private async void OnEditTransactionClicked(object? sender, EventArgs e)
	{
		if(sender is Button { CommandParameter: Guid id })
		{
			await Shell.Current.GoToAsync($"{nameof(EditTransactionPage)}?id={id}");
		}
	}

	private async void OnDeleteTransactionClicked(object? sender, EventArgs e)
	{
		if(sender is not Button { CommandParameter: Guid id })
		{
			return;
		}

		var confirmed = await DisplayAlert(
			"Delete transaction",
            "Are you sure you want to delete this transaction?",
            "Yes",
            "No");

		if (!confirmed)
		{
			return;
		}

		try
		{
			_transactionService.RemoveTransaction(id);
			RefreshTransactions();
			await DisplayAlert("Deleted", "Transaction deleted successfully.", "OK");
		}catch (KeyNotFoundException ex)
		{
			await DisplayAlert("Not found", ex.Message, "OK");
		}
	}

	private void OnShowReportClicked(object? sender, EventArgs e)
	{
		var transactions = _transactionService.GetTransactions();
		var totalIncome = transactions
			.Where(t => t.Type == TransactionType.Income)
			.Sum(t => t.Amount);

		var totalExpense = transactions
			.Where(t => t.Type == TransactionType.Expense)
			.Sum(t => t.Amount);

		TotalIncomeLabel.Text = $"Total Income: {totalIncome:C}";
		TotalExpenseLabel.Text = $"Total Expense: {totalExpense:C}";
		NetBalanceLabel.Text = $"Net Balance: {totalIncome - totalExpense:C}";
		ReportPanel.IsVisible = true;
	}
}