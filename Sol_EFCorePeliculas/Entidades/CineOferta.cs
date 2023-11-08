namespace Sol_EFCorePeliculas.Entidades;
//Relación de uno a uno con Cine
public class CineOferta
{
    public int Id { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public decimal PorcentajeDescuento { get; set; }
    public int CineId { get; set; }
}