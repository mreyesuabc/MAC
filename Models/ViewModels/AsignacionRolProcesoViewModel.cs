using Microsoft.AspNetCore.Mvc.Rendering;

namespace MAC.Models.ViewModels
{
    public class AsignacionRolProcesoViewModel
    {
        public int RolId { get; set; }
        public List<SelectListItem> Roles { get; set; }
        public List<Proceso> ProcesosAsignados { get; set; }
        public List<Proceso> ProcesosDisponibles { get; set; }
    }
}
