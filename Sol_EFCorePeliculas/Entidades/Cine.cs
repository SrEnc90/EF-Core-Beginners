using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Sol_EFCorePeliculas.Entidades;
//Relación de uno a muchos con Sala de Cine
//Relación de uno a uno con CineOferta
public class Cine
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    
    //Si lo dejas así, se muestra un error en swagger al tratar de serializar el Point, por lo que se utiliza el mapeo para extraer la latitud y longitud (Ver servicios/AutoMapperProfiles)
    public Point Ubicacion { get; set; }
    
    public CineOferta CineOferta { get; set; } //Propiedad de navegación
    
    public HashSet<SalaDeCine> SalaDeCines { get; set; } //La ventaja de HashSet con respecto a otros listado es que es más rápido, pero su deventaja es que no ordena
}