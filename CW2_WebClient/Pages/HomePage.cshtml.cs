using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CW2_WebClient.Pages
{
    public class HomePageModel : PageModel
    {
        public string Username { get; set; }
        public IActionResult OnGet()
        {
            Username = HttpContext.Session.GetString("usuario");

            if (string.IsNullOrWhiteSpace(Username))
            {
                return Redirect("/login");
            }

            return Page();
        }

        public IActionResult OnGetLogout()
        {
            HttpContext.Session.Remove("usuario");
            return RedirectToPage("/Login");
        }
    }
}
