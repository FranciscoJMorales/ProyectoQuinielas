using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuinielasModel.DTO
{
    public class UserRegister
    {
        [Required]
        [DisplayName("Nombre de usuario")]
        public string Username { get; set; } = null!;

        [Required]
        [DisplayName("Correo electrónico")]
        public string Email { get; set; } = null!;

        [Required]
        [DisplayName("Contraseña")]
        public string Password { get; set; } = null!;

        [Required]
        [DisplayName("Confirmar Contraseña")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
