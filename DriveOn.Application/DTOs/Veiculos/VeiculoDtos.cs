namespace DriveOn.Application.Veiculos;

public record VeiculoCreateDto(
    long EmpresaId,
    long ClienteId,
    string Placa,
    string? Marca,
    string? Modelo,
    int? Ano,
    string? Observacoes
);

public record VeiculoUpdateDto(
    string Placa,
    string? Marca,
    string? Modelo,
    int? Ano,
    string? Observacoes
);

public record VeiculoListDto(long Id, string Placa, string? Modelo, int? Ano);
public record VeiculoDetailDto(long Id, long EmpresaId, long ClienteId, string Placa, string? Marca, string? Modelo, int? Ano);
