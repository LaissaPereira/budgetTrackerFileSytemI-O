/// <summary>
/// It is Domain model representing a transaction with details such as type, amount, description, date, and time offset.
/// </summary>
 public record Transaction
{
    public Guid Id { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount {get; set;}
    public string Description {get; set;}
    public DateTimeOffset Date { get; set; }
    public DateTimeOffset TimeOffset { get; set; }
    
   
    public Transaction(TransactionType type, decimal amount, string description, DateTimeOffset date, DateTimeOffset timeOffset)
    {
        Id = Guid.NewGuid();
        Type = type;
        Amount = amount;
        Description = description;
        Date = new DateTimeOffset(date.DateTime, timeOffset.Offset);
        TimeOffset = timeOffset;
    }
}