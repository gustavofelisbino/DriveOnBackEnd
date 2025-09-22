namespace DriveOn.Application.Auth;

public record LoginRequest(string Email, string Password, long EmpresaId);
public record LoginByEmailRequest(string Email, string Password);
public record LoginResponse(string Token, long UsuarioId, string Nome, string Cargo, long EmpresaId);