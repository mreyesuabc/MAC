
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAC.Models
{

    [Table("proceso")]

    public class Proceso
    {
        [Key]
        public int Id { get; set; }
        public string descr { get; set; } = string.Empty;
        public string controlador { get; set; } = string.Empty;
        public string accion { get; set; } = string.Empty;
        public string icono { get; set; } = string.Empty;
        public int orden { get; set; } 
        public int activo { get; set; }
        public string? padreId { get; set; }


    }
}
