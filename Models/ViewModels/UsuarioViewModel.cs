using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MAC.Models.ViewModels
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }

        [Required]
        public string UsuarioApp { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Correo { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string ApPaterno { get; set; }

        public string ApMaterno { get; set; }

        [Required]
        public string RFC { get; set; }

        [Required]
        public string Homoclave { get; set; }

        [Required]
        public int Municipio { get; set; }

        [Required]
        public int Rol { get; set; }

        public IEnumerable<SelectListItem> Municipios { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}