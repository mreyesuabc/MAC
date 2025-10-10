namespace MAC.Models.ViewModels
{
    public class PerfilUsuarioViewModel
    {
        public Usuario Usuario { get; set; }
        public CambioPswVm CambioPassword { get; set; } = new CambioPswVm();
    }
}
