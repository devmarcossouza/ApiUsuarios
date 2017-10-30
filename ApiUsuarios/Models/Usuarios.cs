using System;
using System.ComponentModel.DataAnnotations;

namespace ApiUsuarios.Models
{
    public class Usuarios
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nome { get; set; }

        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "E-mail inválido")]
        [Required]
        public string Email { get; set; }
    }
}
