using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Emprendimiento_Api.Models
{
    [Table("Emprendimiento")]
    public class Emprendimiento
    {
        [Key]
        public int IdEmprendimiento { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string ChatWhatsApp {  get; set; }
        public string UrlFoto { get; set; }
        public string Estado { get; set; }
    }
}
