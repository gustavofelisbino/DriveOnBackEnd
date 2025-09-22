namespace DriveOn.Domain.Entities;

public class Fornecedor
{
    public long Id { get; set; }
    public long EmpresaId { get; set; }
    public string Nome { get; set; } = default!;
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public string? Documento { get; set; }
    public string? Observacoes { get; set; }
    public DateTimeOffset CriadoEm { get; set; }
    public DateTimeOffset AtualizadoEm { get; set; }
    public DateTimeOffset? ExcluidoEm { get; set; }
}
