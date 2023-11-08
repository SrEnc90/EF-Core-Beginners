using Sol_EFCorePeliculas.Entidades;

namespace Sol_EFCorePeliculas.DTOs;

public class SalaDeCineCreacionDTO
{
    public decimal Precio { get; set; }
    public TipoSalaDeCine TipoSalaDeCine { get; set; }
}