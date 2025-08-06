using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAC.Models
{
    [Table("usuarioproceso")]
    public class UsuarioProceso
    {
        public int UsuarioId { get; set; }
        public int ProcesoId { get; set; }
    }
}
