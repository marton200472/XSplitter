using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using XSplitter.Models;

namespace XSplitter.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly DbConnection _dbConnection;

    public Expense[] Expenses { get; set; }

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
        _dbConnection = new();
    }

    public async Task OnGetAsync()
    {
        Expenses = (await _dbConnection.GetExpensesAsync()).ToArray();
    }   
}
