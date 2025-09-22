namespace DriveOn.Application.Itens;

public record ItemCreateDto(
    long EmpresaId,
    string? Codigo,
    string Nome,
    string? Descricao,
    string Unidade,
    decimal PrecoUnitario,
    string Tipo
);

public record ItemUpdateDto(
    string? Codigo,
    string Nome,
    string? Descricao,
    string Unidade,
    decimal PrecoUnitario,
    string Tipo
);

public record ItemListDto(long Id, string Nome, decimal PrecoUnitario, string Tipo);
public record ItemDetailDto(long Id, long EmpresaId, string Nome, decimal PrecoUnitario, string Tipo, string? Codigo);
