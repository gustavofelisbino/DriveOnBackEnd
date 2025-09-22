namespace DriveOn.Application.Empresas;

public record EmpresaCreateDto(
    string Nome,
    string? Documento,
    string? Rua,
    string? Numero,
    string? Bairro,
    string? Cep,
    long? CidadeId
);

public record EmpresaUpdateDto(
    string Nome,
    string? Documento,
    string? Rua,
    string? Numero,
    string? Bairro,
    string? Cep,
    long? CidadeId
);

public record EmpresaListDto(long Id, string Nome);
public record EmpresaDetailDto(long Id, string Nome, string? Documento);
