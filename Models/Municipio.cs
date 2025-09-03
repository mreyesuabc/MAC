using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MAC.Models
{

    [Table("municipio")]  

    public class Municipio
    {
        [Key]
        public int mpo { get; set; }
        public string descr { get; set; }

    }
}
