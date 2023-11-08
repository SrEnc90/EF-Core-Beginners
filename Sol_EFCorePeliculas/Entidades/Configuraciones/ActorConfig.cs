using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sol_EFCorePeliculas.Entidades.Configuraciones;

public class ActorConfig: IEntityTypeConfiguration<Actor>
{
    public void Configure(EntityTypeBuilder<Actor> builder)
    {
        builder.Property(prop => prop.Nombre)
            .HasMaxLength(150)
            .IsRequired();
        builder.Property(prop => prop.FechaNacimiento)
            .HasColumnType("Date");
    }
}