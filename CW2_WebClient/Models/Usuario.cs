using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CW2_WebClient.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nome")]
        public string? Nome { get; set; }

        [Required]
        [Display(Name = "Usuário")]
        public string? login { get; set; }

        [Required]
        [Display(Name = "Ativo")]
        public bool Ativo { get; set; }

        [Required]
        [Display(Name = "Senha")]
        public string? Senha { get; set; }

        [Required]
        [Display(Name = "E-mail")]
        public string? Email { get; set; }
    }
}
