namespace ObligatorioP3_frontend.Models
{
    public class ResumenModel
    {
        public int año { get; set; }
        public TipoMovimientoo[] tipoMovimiento { get; set; }
        public int suma { get; set; }
    }
    public class TipoMovimientoo
    {
        public string Nombre { get; set; }
        public int cantidad { get; set; }
    }
}
