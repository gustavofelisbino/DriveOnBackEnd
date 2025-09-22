namespace DriveOn.Application.Financeiro;

public record ContaReceberCreateDto(
    long EmpresaId,
    long ClienteId,
    long? OrdemServicoId,
    string? Descricao,
    decimal ValorTotal,
    int Parcelas,
    DateOnly Vencimento
);

public record ContaPagarCreateDto(
    long EmpresaId,
    long FornecedorId,
    string? Descricao,
    decimal ValorTotal,
    int Parcelas,
    DateOnly Vencimento
);

public record MovimentoCreateDto(
    long EmpresaId,
    string Tipo,
    long? ContaReceberId,
    long? ContaPagarId,
    decimal Valor,
    DateTimeOffset? DataMovimento
);
