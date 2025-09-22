namespace DriveOn.Domain.Entities;

public class Veiculo
{
    public long Id { get; set; }
    public long EmpresaId { get; set; }
    public long ClienteId { get; set; }
    public string Placa { get; set; } = default!;
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public int? Ano { get; set; }
    public string? Observacoes { get; set; }
    public DateTimeOffset CriadoEm { get; set; }
    public DateTimeOffset AtualizadoEm { get; set; }
    public DateTimeOffset? ExcluidoEm { get; set; }
}
