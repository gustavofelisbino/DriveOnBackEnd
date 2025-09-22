namespace DriveOn.Application.Cadastros;

public record ClienteCreateDto(
    long EmpresaId,
    string Nome,
    string? Telefone,
    string? Email,
    string? Cpf,
    string? Rua,
    string? Numero,
    string? Bairro,
    string? Cep,
    long? CidadeId,
    string? Observacoes
);

public record ClienteUpdateDto(
    string Nome,
    string? Telefone,
    string? Email,
    string? Cpf,
    string? Rua,
    string? Numero,
    string? Bairro,
    string? Cep,
    long? CidadeId,
    string? Observacoes
);

public record ClienteListDto(long Id, string Nome, string? Telefone, string? Email);
public record ClienteDetailDto(long Id, long EmpresaId, string Nome, string? Telefone, string? Email);
