
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAC.Models
{

    [Table("proceso")]

    public class Proceso
    {
        [Key]
        public int IdProceso { get; set; }
        public string descr { get; set; } = string.Empty;
    }
}
