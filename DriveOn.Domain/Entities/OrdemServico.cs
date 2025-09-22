namespace DriveOn.Domain.Entities;

public class OrdemServico
{
    public long Id { get; set; }
    public long EmpresaId { get; set; }
    public string Tipo { get; set; } = default!; // servico | manutencao
    public long ClienteId { get; set; }
    public long VeiculoId { get; set; }
    public string Status { get; set; } = "aberta"; // aberta | finalizada | cancelada
    public string? Descricao { get; set; }
    public decimal DescontoValor { get; set; }
    public decimal ValorTotal { get; set; }
    public DateTimeOffset AbertaEm { get; set; }
    public DateTimeOffset? FinalizadaEm { get; set; }
    public long? CriadoPor { get; set; }
    public long? AtualizadoPor { get; set; }
    public DateTimeOffset CriadoEm { get; set; }
    public DateTimeOffset AtualizadoEm { get; set; }
    public DateTimeOffset? ExcluidoEm { get; set; }

    public ICollection<ItemOrdemServico> Itens { get; set; } = new List<ItemOrdemServico>();
}
