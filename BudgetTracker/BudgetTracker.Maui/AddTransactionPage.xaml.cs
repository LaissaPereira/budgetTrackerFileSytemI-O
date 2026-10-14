using System.Globalization;

namespace BudgetTracker.Maui;

public partial class AddTransactionPage : ContentPage
{
    private readonly TransactionService _transactionService;

    public AddTransactionPage(TransactionService transactionService)
    {
        InitializeComponent();
        _transactionService = transactionService;

        TypePicker.ItemsSource = Enum.GetValues<TransactionType>();
        TypePicker.SelectedIndex = 0;
        TransactionDatePicker.Date = DateTime.Today;
    }

    private async void OnAddTransactionClicked(object? sender, EventArgs e)
    {
        if (TypePicker.SelectedItem is not TransactionType type)
        {
            await DisplayAlert("Invalid transaction", "Please select a transaction type.", "OK");
            return;
        }
        if (!decimal.TryParse(AmountEntry.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out var amount))
        {
            await DisplayAlert("Invalid amount", "Please enter a valid amount.", "OK");
            return;
        }
        var description = DescriptionEntry.Text?.Trim() ?? string.Empty;
        var selectedDate = TransactionDatePicker.Date ?? DateTime.Today;
        var transactionDate = new DateTimeOffset(selectedDate);

        var transaction = new Transaction(
            type,
            amount,
            description,
            transactionDate,
            transactionDate);

        try
        {
            _transactionService.AddTransaction(transaction);
            await DisplayAlert("Saved", "Transaction added successfully.", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (ArgumentException ex)
        {
            await DisplayAlert("Invalid transaction", ex.Message, "OK");
        }
        catch (IOException ex)
        {
            await DisplayAlert("Save failed", ex.Message, "OK");
        }
        catch (UnauthorizedAccessException ex)
        {
            await DisplayAlert("Save failed", ex.Message, "OK");
        }
    }
}