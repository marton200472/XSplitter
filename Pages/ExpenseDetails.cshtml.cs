

using System.Collections;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using XSplitter.Models;

namespace XSplitter.Pages;

public class ExpenseDetailsModel : PageModel
{
    public Expense Expense { get; set; }

    public User[] Users { get; set; }

    [BindProperty]
    public Owing[] Owings { get; set; }

    public async Task<IActionResult> OnGetAsync(int id) {
        using var dbConnection = new DbConnection();
        Users = (await dbConnection.GetUsersAsync()).ToArray();
        var exp = await dbConnection.GetExpenseAsync(id);
        if(exp is null) 
            return RedirectToPage(nameof(Index));
        Expense = exp;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id) {
        using var dbConnection = new DbConnection();
        var expense = await dbConnection.GetExpenseAsync(id);
        foreach (var item in Owings)
        {
            item.ExpenseId = id;
        }
        await dbConnection.AddOwings(Owings.Where(x=>x.UserId != expense!.UserId && x.Amount != 0));
        return RedirectToPage(nameof(Index));
    }
}