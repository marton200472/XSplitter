using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace XSplitter.Pages;

public class ClearOwingModel : PageModel
{
    public async Task<IActionResult> OnGetAsync(int userId, int targetId) {
        var db = new DbConnection();
        await db.ClearOwingsForUserAsync(userId, targetId);
        return RedirectToPage(nameof(Index));
    }
}