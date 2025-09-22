namespace DriveOn.Domain.Entities;

public class Usuario
{
    public long Id { get; set; }
    public long EmpresaId { get; set; }
    public string Nome { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string SenhaHash { get; set; } = default!;
    public DateTimeOffset SenhaAtualizadaEm { get; set; }
    public bool Ativo { get; set; } = true;
    public string Cargo { get; set; } = default!; // dono, admin, funcionario, visualizador
    public DateTimeOffset CriadoEm { get; set; }
    public DateTimeOffset AtualizadoEm { get; set; }

    public Empresa? Empresa { get; set; }
}
