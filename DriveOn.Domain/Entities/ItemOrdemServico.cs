namespace DriveOn.Domain.Entities;

public class ItemOrdemServico
{
    public long Id { get; set; }
    public long EmpresaId { get; set; }
    public long OrdemServicoId { get; set; }
    public long? ItemId { get; set; }
    public string? Descricao { get; set; }
    public decimal Quantidade { get; set; } = 1;
    public decimal PrecoUnitario { get; set; } = 0;
    public decimal Total { get; set; } = 0;
    public DateTimeOffset CriadoEm { get; set; }
    public DateTimeOffset AtualizadoEm { get; set; }
}
