namespace XSplitter.Models;

public class Expense
{
    public int Id { get; set; }
    public string Description { get; set; }
    public int Amount { get; set; }
    public int UserId { get; set; }
}