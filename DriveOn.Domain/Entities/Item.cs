namespace DriveOn.Domain.Entities;

public class Item
{
    public long Id { get; set; }
    public long EmpresaId { get; set; }
    public string? Codigo { get; set; }
    public string Nome { get; set; } = default!;
    public string? Descricao { get; set; }
    public string Unidade { get; set; } = "un";
    public decimal PrecoUnitario { get; set; }
    public string Tipo { get; set; } = default!; // servico | produto
    public DateTimeOffset CriadoEm { get; set; }
    public DateTimeOffset AtualizadoEm { get; set; }
    public DateTimeOffset? ExcluidoEm { get; set; }
}
