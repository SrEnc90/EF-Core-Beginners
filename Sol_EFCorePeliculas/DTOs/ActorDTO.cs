namespace Sol_EFCorePeliculas.DTOs;
//DTO=Data Transfer Object(Un objeto que sirve almacenar datos y transferirlos de un lugar a otro)
public class ActorDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public DateTime? FechaNacimiento { get; set; }
}