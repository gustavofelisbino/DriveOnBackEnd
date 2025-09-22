namespace DriveOn.Domain.Entities;

public class ContaPagar
{
    public long Id { get; set; }
    public long EmpresaId { get; set; }
    public long FornecedorId { get; set; }
    public string? Descricao { get; set; }
    public decimal ValorTotal { get; set; }
    public int Parcelas { get; set; } = 1;
    public DateOnly Vencimento { get; set; }
    public string Status { get; set; } = "aberto";
    public DateTimeOffset CriadoEm { get; set; }
    public DateTimeOffset AtualizadoEm { get; set; }
}
