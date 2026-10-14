using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// It is a service class responsible for storing and retrieving transactions from a file.
/// </summary>
public class TransactionStorage
{
    private readonly string _filePath;

    public TransactionStorage(string filePath)
    {
        _filePath = filePath;

        string? folder = Path.GetDirectoryName(_filePath);
        if (folder != null && !Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
    }
    public void SaveTransactions(List<Transaction> transactions)
    {
        var options = new JsonSerializerOptions{ WriteIndented = true, Converters = {new JsonStringEnumConverter()} };
        string json = JsonSerializer.Serialize(transactions, options);
        File.WriteAllText(_filePath, json);
    }

    public List<Transaction> LoadTransactions()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Transaction>();
        }
        try
        {
            string json = File.ReadAllText(_filePath);
            var options = new JsonSerializerOptions{ Converters = {new JsonStringEnumConverter()} };
            var transactions = JsonSerializer.Deserialize<List<Transaction>>(json, options) ?? new List<Transaction>();
            return transactions;
        }
        catch (JsonException)
        {
            Console.WriteLine("Error reading transactions from file.");
            return new List<Transaction>();
        } catch (IOException ex)
        {
            Console.WriteLine($"Error reading transactions from file: {ex.Message}");
            return new List<Transaction>();
        }
    }
}