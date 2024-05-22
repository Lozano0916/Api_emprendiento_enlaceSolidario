using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Emprendimiento_Api.Models
{

    [Table("Producto")]
    public class Products
    {

       [Key]
       public int IdProducto { get; set; }
       public int IdEmprendimiento { get; set; }
       public string NombreProducto { get; set; }
       public string Descripcion { get; set; }
       public decimal precio { get; set; }
       public string UrlFoto { get; set; }
    }
}
