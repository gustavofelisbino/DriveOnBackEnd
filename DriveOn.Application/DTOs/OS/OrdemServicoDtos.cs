namespace DriveOn.Application.OS;

public record ItemOsDto(long? ItemId, string? Descricao, decimal Quantidade, decimal PrecoUnitario);

public record OrdemServicoCriarDto(
    long EmpresaId,
    string Tipo,
    long ClienteId,
    long VeiculoId,
    string? Descricao,
    decimal DescontoValor,
    long? CriadoPor,
    List<ItemOsDto> Itens
);

public record OrdemServicoAtualizarDto(
    string Tipo,
    string Status,
    string? Descricao,
    decimal DescontoValor,
    long? AtualizadoPor
);

public record OrdemServicoDetailDto(
    long Id,
    string Tipo,
    string Status,
    decimal ValorTotal
);
