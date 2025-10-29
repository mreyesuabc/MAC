using MAC.Models.ViewModels;

namespace MAC.Models.ViewModels
{
    public class ProcesoRolViewModel
    {
        public int RolId { get; set; }
        public IEnumerable<ProcesoAsignadoViewModel> ProcesosAsignados { get; set; }
        public IEnumerable<ProcesoNoAsignadoViewModel> ProcesosNoAsignados { get; set; }
    }
}
