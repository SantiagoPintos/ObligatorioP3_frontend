namespace ObligatorioP3_frontend.Models
{
    public class MovimientoModel
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public ArticuloModel Articulo { get; set; }
        public string Usuario { get; set; }
        public int Cantidad { get; set; }
        public TipoMovimientoModel TipoMovimiento { get; set; }
    }
}
