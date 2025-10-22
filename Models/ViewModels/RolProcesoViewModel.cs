using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MAC.Models.ViewModels
{
    public class RolProcesoViewModel
    {
        [Required]
        public int Rol { get; set; }
        public int Proceso { get; set; }


        public IEnumerable<SelectListItem> Roles { get; set; }
        public IEnumerable<SelectListItem> Procesos { get; set; }
    }
}
