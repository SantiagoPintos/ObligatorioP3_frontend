using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics.Contracts;

namespace ObligatorioP3_frontend.Models
{
    public class UsuarioModel
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Clave { get; set; }
        public string ClaveSinEncriptar { get; set; }

    }
}
