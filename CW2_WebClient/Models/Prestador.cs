using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CW2_WebClient.Models
{
    public class Prestador
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Nome")]
        public string? Nome { get; set; }
        [Required]
        [Display(Name = "CGC ou CPF")]
        public string? cgccpf { get; set; }
        [Required]
        [Display(Name = "Ativo")]
        public bool Ativo { get; set; }
    }
}
