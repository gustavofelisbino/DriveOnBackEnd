using DriveOn.Application.Empresas;
using DriveOn.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DriveOn.Api.Controllers;

[ApiController]
[Route("api/empresas")]
public class EmpresasController : ControllerBase
{
    private readonly DriveOnContext _db;
    public EmpresasController(DriveOnContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmpresaListDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var q = _db.Empresas.AsNoTracking().OrderBy(e => e.Nome);
        var items = await q.Skip((page-1)*pageSize).Take(pageSize)
            .Select(e => new EmpresaListDto(e.Id, e.Nome)).ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<EmpresaDetailDto>> GetById(long id)
    {
        var e = await _db.Empresas.FindAsync(id);
        if (e is null) return NotFound();
        return new EmpresaDetailDto(e.Id, e.Nome, e.Documento);
    }

    [HttpPost]
    public async Task<IActionResult> Create(EmpresaCreateDto dto)
    {
        var e = new DriveOn.Domain.Entities.Empresa
        {
            Nome = dto.Nome,
            Documento = dto.Documento,
            Rua = dto.Rua,
            Numero = dto.Numero,
            Bairro = dto.Bairro,
            Cep = dto.Cep,
            CidadeId = dto.CidadeId,
            CriadoEm = DateTimeOffset.UtcNow,
            AtualizadoEm = DateTimeOffset.UtcNow
        };
        _db.Empresas.Add(e);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = e.Id }, new { e.Id });
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, EmpresaUpdateDto dto)
    {
        var e = await _db.Empresas.FindAsync(id);
        if (e is null) return NotFound();
        e.Nome = dto.Nome;
        e.Documento = dto.Documento;
        e.Rua = dto.Rua;
        e.Numero = dto.Numero;
        e.Bairro = dto.Bairro;
        e.Cep = dto.Cep;
        e.CidadeId = dto.CidadeId;
        e.AtualizadoEm = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var e = await _db.Empresas.FindAsync(id);
        if (e is null) return NotFound();
        e.ExcluidoEm = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
