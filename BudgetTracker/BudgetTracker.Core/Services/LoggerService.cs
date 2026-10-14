using System.Globalization;

/// <summary>
/// It is a service class that subscribes to <see cref="TransactionService.TransactionAdded"/>
/// and appends one line per transaction to a log file.
/// </summary>
public class LoggerService
{
    private readonly string _logFilePath;

    public LoggerService(TransactionService transactionService, string logFilePath)
    {
        _logFilePath = logFilePath;

        string? folder = Path.GetDirectoryName(_logFilePath);
        if (!string.IsNullOrEmpty(folder) && !Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        transactionService.TransactionAdded += OnTransactionAdded;
    }

    private void OnTransactionAdded(object? sender, TransactionAdddedEventArgs e)
    {
        var transaction = e.Transaction;
        string line = string.Create(CultureInfo.InvariantCulture,
            $"{transaction.Date:yyyy-MM-dd HH:mm:ss zzz} | {transaction.Type} | {transaction.Amount} | {transaction.Description}{Environment.NewLine}");

        File.AppendAllText(_logFilePath, line);
    }
}
