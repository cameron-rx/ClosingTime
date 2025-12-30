using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HTMXTodo.Pages;

public class IndexModel : PageModel
{
    public void OnGet()
    {

    }

    public IActionResult OnPostItem(string todoItem)
    {
        return Content($"<form><fieldset class='grid'><p>{todoItem}</p><input type='checkbox'></input></fieldset></form>");
    }

}
