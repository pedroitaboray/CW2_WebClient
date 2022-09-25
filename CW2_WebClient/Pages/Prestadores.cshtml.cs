using CW2_WebClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace CW2_WebClient.Pages
{
    public class PrestadoresModel : PageModel
    {
        [BindProperty]
        public Prestador? prestador { get; set; }

        [TempData]
        public bool sucessoCadastro { get; set; } = false;

        public List<Prestador>? prestadores_cadastrados { get; set; }
        public string Username { get; set; }
        public async Task<IActionResult> OnGet([FromServices] IConfiguration config)
        {
            Username = HttpContext.Session.GetString("usuario");

            if (string.IsNullOrWhiteSpace(Username))
            {
                return Redirect("/login");
            }

            await this.AtualizarPrestadoresCadastrados(config);
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
                    config.GetSection("API_Prestador:BaseURL").Value;

                var myContent = JsonConvert.SerializeObject(prestador);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);

                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(baseURL, byteContent);

                response.EnsureSuccessStatusCode();
                sucessoCadastro = true;

                this.LimparTela();

                string conteudo =
                    response.Content.ReadAsStringAsync().Result;
                await this.AtualizarPrestadoresCadastrados(config);
            }

            return Page();
        }

        private void LimparTela()
        {
            prestador.cgccpf = String.Empty;
            prestador.Nome = String.Empty;
        }

        private async Task<bool> AtualizarPrestadoresCadastrados(IConfiguration config)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                string baseURL =
                    config.GetSection("API_Prestador:BaseURL").Value;

                var response = await client.GetAsync(baseURL);
                string conteudo = response.Content.ReadAsStringAsync().Result;
                var myContent = JsonConvert.DeserializeObject<List<Prestador>>(conteudo);
                prestadores_cadastrados = myContent;
            }

            return true;
        }
    }
}
