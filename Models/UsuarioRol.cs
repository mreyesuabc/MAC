using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAC.Models
{
    [Table("usuariorol")]
    public class UsuarioRol
    {
        public int UsuarioId { get; set; }
        public int RolId { get; set; }
    }
}
