using Microsoft.EntityFrameworkCore;
using DriveOn.Domain.Entities;

namespace DriveOn.Infrastructure.Persistence;

public class DriveOnContext : DbContext
{
    public const string Schema = "driveon";

    public DriveOnContext(DbContextOptions<DriveOnContext> options) : base(options) {}

    public DbSet<Estado> Estados => Set<Estado>();
    public DbSet<Cidade> Cidades => Set<Cidade>();
    public DbSet<Empresa> Empresas => Set<Empresa>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Veiculo> Veiculos => Set<Veiculo>();
    public DbSet<Item> Itens => Set<Item>();
    public DbSet<Fornecedor> Fornecedores => Set<Fornecedor>();
    public DbSet<OrdemServico> OrdensServico => Set<OrdemServico>();
    public DbSet<ItemOrdemServico> ItensOrdemServico => Set<ItemOrdemServico>();
    public DbSet<ContaReceber> ContasReceber => Set<ContaReceber>();
    public DbSet<ContaPagar> ContasPagar => Set<ContaPagar>();
    public DbSet<MovimentoFinanceiro> MovimentosFinanceiros => Set<MovimentoFinanceiro>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);

        // estados
        modelBuilder.Entity<Cidade>(e =>
        {
            e.ToTable("cidades");
            e.HasKey(x => x.Id);

            e.HasOne(x => x.Estado)            // use a navigation explícita
                .WithMany(s => s.Cidades)
                .HasForeignKey(x => x.EstadoId)   // e o FK explícito
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => new { x.EstadoId, x.Nome }).IsUnique();
        });

// Empresa ↔ Cidade (muitos-para-um opcional)
        modelBuilder.Entity<Empresa>(e =>
        {
            e.ToTable("empresas");
            e.HasKey(x => x.Id);

            e.HasOne(x => x.Cidade)
                .WithMany()                       // sem coleção do outro lado
                .HasForeignKey(x => x.CidadeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

// Usuario ↔ Empresa (muitos-para-um)
        modelBuilder.Entity<Usuario>(e =>
        {
            e.ToTable("usuarios");
            e.HasKey(x => x.Id);

            e.Property(x => x.Email).IsRequired();
            e.Property(x => x.Cargo).IsRequired();
            e.HasIndex(x => new { x.EmpresaId, x.Email }).IsUnique();

            e.HasOne(x => x.Empresa)
                .WithMany(emp => emp.Usuarios)
                .HasForeignKey(x => x.EmpresaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Cliente>(e =>
        {
            e.ToTable("clientes");
            e.HasKey(x => x.Id);
            e.HasOne<Cidade>().WithMany().HasForeignKey(x => x.CidadeId);
        });

        modelBuilder.Entity<Veiculo>(e =>
        {
            e.ToTable("veiculos");
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.EmpresaId, x.Placa }).IsUnique();
        });

        modelBuilder.Entity<Item>(e =>
        {
            e.ToTable("itens");
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.EmpresaId, x.Codigo }).IsUnique();
        });

        modelBuilder.Entity<Fornecedor>(e =>
        {
            e.ToTable("fornecedores");
            e.HasKey(x => x.Id);
        });

        modelBuilder.Entity<OrdemServico>(e =>
        {
            e.ToTable("ordens_servico");
            e.HasKey(x => x.Id);
            e.Property(x => x.Status).HasDefaultValue("aberta");
        });

        modelBuilder.Entity<ItemOrdemServico>(e =>
        {
            e.ToTable("itens_ordem_servico");
            e.HasKey(x => x.Id);
            e.HasOne<OrdemServico>().WithMany(os => os.Itens).HasForeignKey(x => x.OrdemServicoId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ContaReceber>(e =>
        {
            e.ToTable("contas_receber");
            e.HasKey(x => x.Id);
        });

        modelBuilder.Entity<ContaPagar>(e =>
        {
            e.ToTable("contas_pagar");
            e.HasKey(x => x.Id);
        });

        modelBuilder.Entity<MovimentoFinanceiro>(e =>
        {
            e.ToTable("movimentos_financeiros");
            e.HasKey(x => x.Id);
        });

        base.OnModelCreating(modelBuilder);
    }
}
