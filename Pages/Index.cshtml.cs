using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using XSplitter.Models;

namespace XSplitter.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly DbConnection _dbConnection;

    public Expense[] Expenses { get; set; }

    public Dictionary<User,List<OwingToUser>> Owings { get; set; } = new();

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
        _dbConnection = new();
    }

    public async Task OnGetAsync()
    {
        Expenses = (await _dbConnection.GetExpensesAsync()).ToArray();
        var users = await _dbConnection.GetUsersAsync();
        foreach (var u in users)
        {
            var owings = (await _dbConnection.GetOwingsForUser(u.Id)).ToList();
            Owings.Add(u, owings);
        }
    }   
}
