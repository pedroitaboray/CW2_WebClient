using CW2_WebClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CW2_WebClient.Pages
{
    public class UsuariosModel : PageModel
    {
        [BindProperty]
        public Usuario? usuario { get; set; }
        public void OnGet()
        {
        }
    }
}
