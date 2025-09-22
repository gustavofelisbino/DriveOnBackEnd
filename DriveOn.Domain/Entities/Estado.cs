namespace DriveOn.Domain.Entities;

public class Estado
{
    public long Id { get; set; }
    public string Nome { get; set; } = default!;
    public string Sigla { get; set; } = default!;
    public DateTimeOffset CriadoEm { get; set; }
    public DateTimeOffset AtualizadoEm { get; set; }
    public ICollection<Cidade> Cidades { get; set; } = new List<Cidade>();
}
