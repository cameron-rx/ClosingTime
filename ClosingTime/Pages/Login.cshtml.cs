using AspNet.Security.OAuth.Discord;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HTMXTodo.Pages;

public class LoginModel : PageModel
{
    public void OnGet()
    {

    }

    public IActionResult OnPost()
    {
        return Challenge(new AuthenticationProperties 
        { 
            RedirectUri = "/" // Where to go after Discord says "Yes"
        }, DiscordAuthenticationDefaults.AuthenticationScheme);
    }
    

}