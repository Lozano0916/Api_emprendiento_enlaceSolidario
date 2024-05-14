using System.ComponentModel.DataAnnotations;

namespace Emprendimiento_Api.Models
{
    public class Usuarios
    {
        [Key]
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
    }
}
