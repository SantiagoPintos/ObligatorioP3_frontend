namespace ObligatorioP3_frontend.Models
{
    public class ArticuloModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public long Codigo { get; set; }
        public string Descripcion { get; set; }
        public int stock { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
