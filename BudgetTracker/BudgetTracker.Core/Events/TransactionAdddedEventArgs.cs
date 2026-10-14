/// <summary>   
/// It is an event arguments class representing the addition of a new transaction.
/// </summary>
public class TransactionAdddedEventArgs : EventArgs
{
    public Transaction Transaction { get; }

    public TransactionAdddedEventArgs(Transaction transaction)
    {
        Transaction = transaction;
    }
}