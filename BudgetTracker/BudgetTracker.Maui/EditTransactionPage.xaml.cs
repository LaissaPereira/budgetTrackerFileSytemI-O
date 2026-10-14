using System.Globalization;

namespace BudgetTracker.Maui;

[QueryProperty(nameof(TransactionId), "id")]
public partial class EditTransactionPage : ContentPage
{
    private readonly TransactionService _transactionService;
    private Guid _transactionId;

    public string TransactionId
    {
        set
        {
            if(Guid.TryParse(value, out var id))
            {
                _transactionId = id;
                LoadTransaction();
            }
        }
    }

    public EditTransactionPage(TransactionService transactionService)
    {
        InitializeComponent();
        _transactionService = transactionService;
        TypePicker.ItemsSource = Enum.GetValues<TransactionType>();
    
    }

    private async void LoadTransaction()
    {
        var transaction =  _transactionService.GetTransactions().FirstOrDefault(t => t.Id == _transactionId);
        if (transaction == null)
        {
            return;
        }
        TypePicker.SelectedItem = transaction.Type;
        AmountEntry.Text = transaction.Amount.ToString(CultureInfo.CurrentCulture);
        DescriptionEntry.Text = transaction.Description;
        TransactionDatePicker.Date = transaction.Date.Date;
    }

    private async void OnUpdateTransactionClicked(object? sender, EventArgs e)
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
        var transactionDate = new DateTimeOffset(TransactionDatePicker.Date ?? DateTime.Today);

        var updatedTransaction = new Transaction(
            type,
            amount,
            description,
            transactionDate,
            transactionDate);

        try
        {
            _transactionService.UpdateTransaction(_transactionId,updatedTransaction);
            await DisplayAlert("Saved", "Transaction updated successfully.", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (ArgumentException ex)
        {
            await DisplayAlert("Invalid transaction", ex.Message, "OK");
        }
        catch (KeyNotFoundException ex)
        {
            await DisplayAlert("Not found", ex.Message, "OK");
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