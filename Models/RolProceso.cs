using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAC.Models
{
    [Table("rolProceso")]
    public class RolProceso
    {
        public int RolId { get; set; }
        [NotMapped]
        public Rol rolInfo { get; set; }
        public int ProcesoId { get; set; }
     
        [NotMapped]
        public Proceso procesoInfo { get; set; }
    }
}
