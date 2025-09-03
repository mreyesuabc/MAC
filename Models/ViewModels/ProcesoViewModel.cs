namespace MAC.Models.ViewModels
{
    public class ProcesoViewModel
    {
        public int Id { get; set; }
        public string descr { get; set; }
        public string controlador { get; set; }
        public string accion { get; set; }
        public string icono { get; set; }
        public int orden { get; set; }
        public int activo { get; set; }
        public string padreId { get; set; }
    }
}