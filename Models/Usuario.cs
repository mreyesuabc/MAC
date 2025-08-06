using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAC.Models
{
    [Table("usuario")]
    public class Usuario
    {
        [Key]
        public int id { get; set; }
        public string usuarioapp { get; set; } 
        public string password { get; set; }
        public string correo { get; set; } 
        public string nombre { get; set; } 
        public string appaterno { get; set; }
        public string apmaterno { get; set; } 
        public string rfc { get; set; } 
        public string homoclave { get; set; } 
        public int municipio { get; set; }



    }
}
