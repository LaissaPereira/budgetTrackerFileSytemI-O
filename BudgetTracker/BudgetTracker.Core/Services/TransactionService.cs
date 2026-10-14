using System.Data;

/// <summary>
/// TransitionService are handling operations related to transactions and call the storage where data is persisted.
/// </summary>
public class TransactionService
{
    private readonly TransactionStorage _transactionStorage;

    /// <summary>
    /// Raised after a transaction has been successfully added and persisted.
    /// </summary>
    public event EventHandler<TransactionAdddedEventArgs>? TransactionAdded;

    public TransactionService(TransactionStorage transactionStorage)
    {
        _transactionStorage = transactionStorage;
    }

    public IReadOnlyList<Transaction> GetTransactions()
    {
        return _transactionStorage.LoadTransactions();
    }

    public void AddTransaction(Transaction transaction)
    {
        // Validate the transaction before adding it
        if(transaction.Amount <= 0)
        {
            throw new ArgumentException("Transaction amount must be greater than zero.");
        }
        if(string.IsNullOrWhiteSpace(transaction.Description))
        {
            throw new ArgumentException("Transaction description cannot be empty.");
        }
        if(transaction.Date == default)
        {
            throw new ArgumentException("Transaction date is not valid.");
        }
        if(transaction.Type != TransactionType.Income && transaction.Type != TransactionType.Expense)
        {
            throw new ArgumentException("Transaction type is not valid.");
        }
        var transactions = _transactionStorage.LoadTransactions();
        transactions.Add(transaction);
        _transactionStorage.SaveTransactions(transactions);
        TransactionAdded?.Invoke(this, new TransactionAdddedEventArgs(transaction));
    }

    public void RemoveTransaction(Guid transactionId)
    {
        var transactions = _transactionStorage.LoadTransactions();
        var transactionToRemove = transactions.FirstOrDefault(t => t.Id == transactionId);
        if(transactionToRemove == null)
        {
            throw new KeyNotFoundException($"Transaction with Id '{transactionId}' was not found.");
        }
        transactions.Remove(transactionToRemove);
        _transactionStorage.SaveTransactions(transactions);
    }

    public void UpdateTransaction(Guid transactionId, Transaction updatedTransaction)
    {
        // Validate the updated transaction before applying changes
        if(updatedTransaction.Amount <= 0)
        {
            throw new ArgumentException("Transaction amount must be greater than zero.");
        }
        if(string.IsNullOrWhiteSpace(updatedTransaction.Description))
        {
            throw new ArgumentException("Transaction description cannot be empty.");
        }
        if(updatedTransaction.Date == default)
        {
            throw new ArgumentException("Transaction date is not valid.");
        }
        if(updatedTransaction.Type != TransactionType.Income && updatedTransaction.Type != TransactionType.Expense)
        {
            throw new ArgumentException("Transaction type is not valid.");
        }

        var transactions = _transactionStorage.LoadTransactions();
        var transactionToUpdate = transactions.FirstOrDefault(t => t.Id == transactionId);
        if(transactionToUpdate == null)
        {
            throw new KeyNotFoundException($"Transaction with Id '{transactionId}' was not found.");
        }
        // Preserve the original Id, since the constructor always generates a new one.
        updatedTransaction.Id = transactionId;
        var index = transactions.IndexOf(transactionToUpdate);
        transactions[index] = updatedTransaction;
        _transactionStorage.SaveTransactions(transactions);
    }

   
}
