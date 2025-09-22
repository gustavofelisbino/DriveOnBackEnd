namespace DriveOn.Domain.Entities;

public class Empresa
{
    public long Id { get; set; }
    public string Nome { get; set; } = default!;
    public string? Documento { get; set; }
    public string? Rua { get; set; }
    public string? Numero { get; set; }
    public string? Bairro { get; set; }
    public string? Cep { get; set; }
    public long? CidadeId { get; set; }
    public DateTimeOffset CriadoEm { get; set; }
    public DateTimeOffset AtualizadoEm { get; set; }
    public DateTimeOffset? ExcluidoEm { get; set; }

    public Cidade? Cidade { get; set; }
    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
