namespace DriveOn.Application.Usuarios;

public record UsuarioCreateDto(
    long EmpresaId,
    string Nome,
    string Email,
    string Password,
    string Cargo
);

public record UsuarioUpdateDto(
    string Nome,
    string Email,
    string? NewPassword,
    string Cargo,
    bool Ativo
);

public record UsuarioListDto(long Id, string Nome, string Email, string Cargo, bool Ativo);
public record UsuarioDetailDto(long Id, long EmpresaId, string Nome, string Email, string Cargo, bool Ativo);
