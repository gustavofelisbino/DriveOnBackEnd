using DriveOn.Application.Financeiro;
using DriveOn.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DriveOn.Api.Controllers;

[ApiController]
[Route("api/financeiro")]
public class FinanceiroController : ControllerBase
{
    private readonly DriveOnContext _db;
    public FinanceiroController(DriveOnContext db) => _db = db;

    [HttpPost("contas-receber")]
    public async Task<IActionResult> CriarContaReceber(ContaReceberCreateDto dto)
    {
        var cr = new DriveOn.Domain.Entities.ContaReceber
        {
            EmpresaId = dto.EmpresaId,
            ClienteId = dto.ClienteId,
            OrdemServicoId = dto.OrdemServicoId,
            Descricao = dto.Descricao,
            ValorTotal = dto.ValorTotal,
            Parcelas = dto.Parcelas,
            Vencimento = dto.Vencimento,
            Status = "aberto",
            CriadoEm = DateTimeOffset.UtcNow,
            AtualizadoEm = DateTimeOffset.UtcNow
        };
        _db.ContasReceber.Add(cr);
        await _db.SaveChangesAsync();
        return Created($"api/financeiro/contas-receber/{cr.Id}", new { cr.Id });
    }

    [HttpPost("contas-pagar")]
    public async Task<IActionResult> CriarContaPagar(ContaPagarCreateDto dto)
    {
        var cp = new DriveOn.Domain.Entities.ContaPagar
        {
            EmpresaId = dto.EmpresaId,
            FornecedorId = dto.FornecedorId,
            Descricao = dto.Descricao,
            ValorTotal = dto.ValorTotal,
            Parcelas = dto.Parcelas,
            Vencimento = dto.Vencimento,
            Status = "aberto",
            CriadoEm = DateTimeOffset.UtcNow,
            AtualizadoEm = DateTimeOffset.UtcNow
        };
        _db.ContasPagar.Add(cp);
        await _db.SaveChangesAsync();
        return Created($"api/financeiro/contas-pagar/{cp.Id}", new { cp.Id });
    }

    [HttpPost("movimentos")]
    public async Task<IActionResult> RegistrarMovimento(MovimentoCreateDto dto)
    {
        if (dto.Tipo == "recebimento" && dto.ContaReceberId is null) return BadRequest("conta_receber_id obrigatório para recebimento");
        if (dto.Tipo == "pagamento" && dto.ContaPagarId is null) return BadRequest("conta_pagar_id obrigatório para pagamento");

        var mov = new DriveOn.Domain.Entities.MovimentoFinanceiro
        {
            EmpresaId = dto.EmpresaId,
            Tipo = dto.Tipo,
            ContaReceberId = dto.ContaReceberId,
            ContaPagarId = dto.ContaPagarId,
            Valor = dto.Valor,
            DataMovimento = dto.DataMovimento ?? DateTimeOffset.UtcNow,
            CriadoEm = DateTimeOffset.UtcNow,
            AtualizadoEm = DateTimeOffset.UtcNow
        };
        _db.MovimentosFinanceiros.Add(mov);

        if (dto.Tipo == "recebimento" && dto.ContaReceberId is not null)
        {
            var cr = await _db.ContasReceber.FirstOrDefaultAsync(x => x.Id == dto.ContaReceberId);
            if (cr is not null)
            {
                var totalRecebido = await _db.MovimentosFinanceiros.Where(m => m.ContaReceberId == cr.Id).SumAsync(m => m.Valor);
                var novoTotal = totalRecebido + dto.Valor;
                cr.Status = novoTotal >= cr.ValorTotal ? "pago" : "parcial";
                cr.AtualizadoEm = DateTimeOffset.UtcNow;
            }
        }

        if (dto.Tipo == "pagamento" && dto.ContaPagarId is not null)
        {
            var cp = await _db.ContasPagar.FirstOrDefaultAsync(x => x.Id == dto.ContaPagarId);
            if (cp is not null)
            {
                var totalPago = await _db.MovimentosFinanceiros.Where(m => m.ContaPagarId == cp.Id).SumAsync(m => m.Valor);
                var novoTotal = totalPago + dto.Valor;
                cp.Status = novoTotal >= cp.ValorTotal ? "pago" : "parcial";
                cp.AtualizadoEm = DateTimeOffset.UtcNow;
            }
        }

        await _db.SaveChangesAsync();
        return Created($"api/financeiro/movimentos/{mov.Id}", new { mov.Id });
    }
}
