using DriveOn.Application.OS;
using DriveOn.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DriveOn.Api.Controllers;

[ApiController]
[Route("api/os")]
public class OrdemServicoController : ControllerBase
{
    private readonly DriveOnContext _db;
    public OrdemServicoController(DriveOnContext db) => _db = db;

    [HttpPost]
    public async Task<ActionResult<OrdemServicoDetailDto>> Create(OrdemServicoCriarDto dto)
    {
        var os = new DriveOn.Domain.Entities.OrdemServico
        {
            EmpresaId = dto.EmpresaId,
            Tipo = dto.Tipo,
            ClienteId = dto.ClienteId,
            VeiculoId = dto.VeiculoId,
            Descricao = dto.Descricao,
            DescontoValor = dto.DescontoValor,
            Status = "aberta",
            AbertaEm = DateTimeOffset.UtcNow,
            CriadoPor = dto.CriadoPor,
            CriadoEm = DateTimeOffset.UtcNow,
            AtualizadoEm = DateTimeOffset.UtcNow
        };

        decimal total = 0;
        foreach (var it in dto.Itens)
        {
            var preco = it.PrecoUnitario;
            if (it.ItemId.HasValue)
            {
                var itemDb = await _db.Itens.AsNoTracking().FirstOrDefaultAsync(i => i.Id == it.ItemId.Value);
                if (itemDb is not null) preco = itemDb.PrecoUnitario;
            }
            var lineTotal = it.Quantidade * preco;
            total += lineTotal;
            os.Itens.Add(new DriveOn.Domain.Entities.ItemOrdemServico
            {
                EmpresaId = dto.EmpresaId,
                ItemId = it.ItemId,
                Descricao = it.Descricao,
                Quantidade = it.Quantidade,
                PrecoUnitario = preco,
                Total = lineTotal,
                CriadoEm = DateTimeOffset.UtcNow,
                AtualizadoEm = DateTimeOffset.UtcNow
            });
        }

        os.ValorTotal = Math.Max(0, total - os.DescontoValor);
        _db.OrdensServico.Add(os);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = os.Id }, new OrdemServicoDetailDto(os.Id, os.Tipo, os.Status, os.ValorTotal));
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<OrdemServicoDetailDto>> GetById(long id)
    {
        var os = await _db.OrdensServico.Include(x => x.Itens).FirstOrDefaultAsync(x => x.Id == id);
        if (os is null) return NotFound();
        return new OrdemServicoDetailDto(os.Id, os.Tipo, os.Status, os.ValorTotal);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, OrdemServicoAtualizarDto dto)
    {
        var os = await _db.OrdensServico.Include(x => x.Itens).FirstOrDefaultAsync(x => x.Id == id);
        if (os is null) return NotFound();
        os.Tipo = dto.Tipo;
        os.Status = dto.Status;
        os.Descricao = dto.Descricao;
        os.DescontoValor = dto.DescontoValor;
        os.AtualizadoPor = dto.AtualizadoPor;
        os.AtualizadoEm = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
