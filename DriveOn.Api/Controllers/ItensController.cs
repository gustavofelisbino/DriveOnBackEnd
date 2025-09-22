using DriveOn.Application.Itens;
using DriveOn.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DriveOn.Api.Controllers;

[ApiController]
[Route("api/itens")]
public class ItensController : ControllerBase
{
    private readonly DriveOnContext _db;
    public ItensController(DriveOnContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItemListDto>>> GetAll([FromQuery] long empresaId, [FromQuery] string? q)
    {
        var query = _db.Itens.AsNoTracking().Where(i => i.EmpresaId == empresaId && i.ExcluidoEm == null);
        if (!string.IsNullOrWhiteSpace(q)) query = query.Where(i => i.Nome.ToLower().Contains(q.ToLower()));
        var items = await query.OrderBy(i => i.Nome).Select(i => new ItemListDto(i.Id, i.Nome, i.PrecoUnitario, i.Tipo)).ToListAsync();
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ItemCreateDto dto)
    {
        var i = new DriveOn.Domain.Entities.Item
        {
            EmpresaId = dto.EmpresaId,
            Codigo = dto.Codigo,
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            Unidade = dto.Unidade,
            PrecoUnitario = dto.PrecoUnitario,
            Tipo = dto.Tipo,
            CriadoEm = DateTimeOffset.UtcNow,
            AtualizadoEm = DateTimeOffset.UtcNow
        };
        _db.Itens.Add(i);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = i.Id }, new { i.Id });
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, ItemUpdateDto dto)
    {
        var i = await _db.Itens.FindAsync(id);
        if (i is null) return NotFound();
        i.Codigo = dto.Codigo;
        i.Nome = dto.Nome;
        i.Descricao = dto.Descricao;
        i.Unidade = dto.Unidade;
        i.PrecoUnitario = dto.PrecoUnitario;
        i.Tipo = dto.Tipo;
        i.AtualizadoEm = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var i = await _db.Itens.FindAsync(id);
        if (i is null) return NotFound();
        i.ExcluidoEm = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
