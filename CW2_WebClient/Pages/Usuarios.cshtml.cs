using CW2_WebClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace CW2_WebClient.Pages
{
    public class UsuariosModel : PageModel
    {
        [BindProperty]
        public Usuario? usuario { get; set; }

        public List<Usuario>? usuarios_cadastrados { get; set; }

        [TempData]
        public bool sucessoCadastro { get; set; } = false;

        public string Username { get; set; }
        public  async Task<IActionResult> OnGet([FromServices] IConfiguration config)
        {
            Username = HttpContext.Session.GetString("usuario");

            if (string.IsNullOrWhiteSpace(Username))
            {
                return Redirect("/login");
            }

            await this.AtualizarUsuariosCadastrados(config);
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

                usuario.Senha = GerarMD5(usuario.Senha);

                var myContent = JsonConvert.SerializeObject(usuario);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);

                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(baseURL, byteContent);

                response.EnsureSuccessStatusCode();
                sucessoCadastro = true;

                string conteudo =
                    response.Content.ReadAsStringAsync().Result;
                await this.AtualizarUsuariosCadastrados(config);
            }

            return Page();
        }

        private async Task<bool> AtualizarUsuariosCadastrados(IConfiguration config)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                string baseURL =
                    config.GetSection("API_Usuario:BaseURL").Value;

                var response = await client.GetAsync(baseURL);
                string conteudo = response.Content.ReadAsStringAsync().Result;
                var myContent = JsonConvert.DeserializeObject<List<Usuario>>(conteudo);
                usuarios_cadastrados = myContent;
            }

            return true;
        }

        public string GerarMD5(string valor)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] valorCriptografado = md5Hasher.ComputeHash(Encoding.Default.GetBytes(valor));
            StringBuilder strBuilder = new StringBuilder();

            for (int i = 0; i < valorCriptografado.Length; i++)
            {
                strBuilder.Append(valorCriptografado[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }
}
