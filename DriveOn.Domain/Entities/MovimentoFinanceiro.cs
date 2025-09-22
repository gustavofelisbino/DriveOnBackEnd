namespace DriveOn.Domain.Entities;

public class MovimentoFinanceiro
{
    public long Id { get; set; }
    public long EmpresaId { get; set; }
    public string Tipo { get; set; } = default!; // recebimento | pagamento
    public long? ContaReceberId { get; set; }
    public long? ContaPagarId { get; set; }
    public decimal Valor { get; set; }
    public DateTimeOffset DataMovimento { get; set; }
    public DateTimeOffset CriadoEm { get; set; }
    public DateTimeOffset AtualizadoEm { get; set; }
}
