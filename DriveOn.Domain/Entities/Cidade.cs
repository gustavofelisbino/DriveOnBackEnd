namespace DriveOn.Domain.Entities;

public class Cidade
{
    public long Id { get; set; }
    public long EstadoId { get; set; }
    public string Nome { get; set; } = default!;
    public DateTimeOffset CriadoEm { get; set; }
    public DateTimeOffset AtualizadoEm { get; set; }

    public Estado? Estado { get; set; }
}
