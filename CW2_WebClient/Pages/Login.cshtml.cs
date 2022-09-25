using CW2_WebClient.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

namespace CW2_WebClient.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly Data.LoginDbContext _context;

        public LoginModel(Data.LoginDbContext context)
        {
            _context = context;
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Login? Login { get; set; }

        [TempData]
        public bool errologin { get; set; } = false;

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
                    config.GetSection("API_Login:BaseURL").Value;

                Login.senha = GerarMD5(Login.senha);

                var myContent = JsonConvert.SerializeObject(Login);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);

                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(baseURL, byteContent);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    errologin = true;
                    return Page();
                }

                response.EnsureSuccessStatusCode();
                errologin = false;
                string conteudo =
                    response.Content.ReadAsStringAsync().Result;

                HttpContext.Session.SetString("usuario", Login.login);
            }

            return Redirect("/HomePage");
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
