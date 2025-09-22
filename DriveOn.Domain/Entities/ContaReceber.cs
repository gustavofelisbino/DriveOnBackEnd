namespace DriveOn.Domain.Entities;

public class ContaReceber
{
    public long Id { get; set; }
    public long EmpresaId { get; set; }
    public long ClienteId { get; set; }
    public long? OrdemServicoId { get; set; }
    public string? Descricao { get; set; }
    public decimal ValorTotal { get; set; }
    public int Parcelas { get; set; } = 1;
    public DateOnly Vencimento { get; set; }
    public string Status { get; set; } = "aberto"; // aberto, parcial, pago
    public DateTimeOffset CriadoEm { get; set; }
    public DateTimeOffset AtualizadoEm { get; set; }
}
