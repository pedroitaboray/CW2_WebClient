using System.ComponentModel.DataAnnotations;

namespace CW2_WebClient.Models
{
    public class Login
    {
        [Required]
        [Display(Name = "Usuário")]
        public string? login { get; set; }

        [Required]
        [Display(Name = "Senha")]
        public string? senha { get; set; }
    }
}
