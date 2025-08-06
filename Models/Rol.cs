using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MAC.Models
{
    [Table("rol")]
    public class Rol
    {
        [Key]
        public int Id { get; set; }
        public string descr { get; set; } = string.Empty;
    }
}
