

using System.Collections;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using XSplitter.Models;

namespace XSplitter.Pages;

public class AddExpenseModel : PageModel
{



    [BindProperty]
    public Expense Expense { get; set; }

    public IEnumerable<User> Users { get; set; }

    public async Task OnGetAsync() {
        using var dbConnection = new DbConnection();
        Users = await dbConnection.GetUsersAsync();
    }

    public async Task<IActionResult> OnPostAsync() {
        using var dbConnection = new DbConnection();
        var expenseId = await dbConnection.AddExpense(Expense);
        return RedirectToPage("ExpenseDetails",new {id=expenseId});
    }
}