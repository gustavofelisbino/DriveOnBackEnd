using DriveOn.Application.Veiculos;
using DriveOn.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DriveOn.Api.Controllers;

[ApiController]
[Route("api/veiculos")]
public class VeiculosController : ControllerBase
{
    private readonly DriveOnContext _db;
    public VeiculosController(DriveOnContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VeiculoListDto>>> GetAll([FromQuery] long empresaId, [FromQuery] long? clienteId, [FromQuery] string? placa)
    {
        var q = _db.Veiculos.AsNoTracking().Where(v => v.EmpresaId == empresaId && v.ExcluidoEm == null);
        if (clienteId is not null) q = q.Where(v => v.ClienteId == clienteId);
        if (!string.IsNullOrWhiteSpace(placa)) q = q.Where(v => v.Placa.ToLower().Contains(placa.ToLower()));

        var items = await q.OrderBy(v => v.Placa).Select(v => new VeiculoListDto(v.Id, v.Placa, v.Modelo, v.Ano ?? 0)).ToListAsync();
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create(VeiculoCreateDto dto)
    {
        var v = new DriveOn.Domain.Entities.Veiculo
        {
            EmpresaId = dto.EmpresaId,
            ClienteId = dto.ClienteId,
            Placa = dto.Placa,
            Marca = dto.Marca,
            Modelo = dto.Modelo,
            Ano = dto.Ano,
            Observacoes = dto.Observacoes,
            CriadoEm = DateTimeOffset.UtcNow,
            AtualizadoEm = DateTimeOffset.UtcNow
        };
        _db.Veiculos.Add(v);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = v.Id }, new { v.Id });
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, VeiculoUpdateDto dto)
    {
        var v = await _db.Veiculos.FindAsync(id);
        if (v is null) return NotFound();
        v.Placa = dto.Placa;
        v.Marca = dto.Marca;
        v.Modelo = dto.Modelo;
        v.Ano = dto.Ano;
        v.Observacoes = dto.Observacoes;
        v.AtualizadoEm = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var v = await _db.Veiculos.FindAsync(id);
        if (v is null) return NotFound();
        v.ExcluidoEm = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
