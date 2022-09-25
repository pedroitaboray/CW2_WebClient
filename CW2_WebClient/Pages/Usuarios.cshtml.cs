using CW2_WebClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace CW2_WebClient.Pages
{
    public class UsuariosModel : PageModel
    {
        [BindProperty]
        public Usuario? usuario { get; set; }

        [TempData]
        public bool sucessoCadastro { get; set; } = false;

        public string Username { get; set; }
        public IActionResult OnGet()
        {
            Username = HttpContext.Session.GetString("usuario");

            if (string.IsNullOrWhiteSpace(Username))
            {
                return Redirect("/login");
            }

            sucessoCadastro = false;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromServices] IConfiguration config)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                string baseURL =
                    config.GetSection("API_Usuario:BaseURL").Value;

                var myContent = JsonConvert.SerializeObject(usuario);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);

                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(baseURL, byteContent);

                response.EnsureSuccessStatusCode();
                sucessoCadastro = true;

                string conteudo =
                    response.Content.ReadAsStringAsync().Result;
            }

            return Page();
        }
    }
}
