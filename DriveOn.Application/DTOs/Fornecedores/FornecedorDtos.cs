namespace DriveOn.Application.Fornecedores;

public record FornecedorCreateDto(
    long EmpresaId,
    string Nome,
    string? Email,
    string? Telefone,
    string? Documento,
    string? Observacoes
);

public record FornecedorUpdateDto(
    string Nome,
    string? Email,
    string? Telefone,
    string? Documento,
    string? Observacoes
);

public record FornecedorListDto(long Id, string Nome, string? Telefone);
public record FornecedorDetailDto(long Id, long EmpresaId, string Nome, string? Telefone, string? Email);
