namespace DriveOn.Domain.Entities;

public class Cliente
{
    public long Id { get; set; }
    public long EmpresaId { get; set; }
    public string Nome { get; set; } = default!;
    public string? Telefone { get; set; }
    public string? Email { get; set; }
    public string? Cpf { get; set; }
    public string? Rua { get; set; }
    public string? Numero { get; set; }
    public string? Bairro { get; set; }
    public string? Cep { get; set; }
    public long? CidadeId { get; set; }
    public string? Observacoes { get; set; }
    public DateTimeOffset CriadoEm { get; set; }
    public DateTimeOffset AtualizadoEm { get; set; }
    public DateTimeOffset? ExcluidoEm { get; set; }
}
