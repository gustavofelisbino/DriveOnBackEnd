using DriveOn.Application.Cadastros;
using DriveOn.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DriveOn.Api.Controllers;

[ApiController]
[Route("api/clientes")]
public class ClientesController : ControllerBase
{
    private readonly DriveOnContext _db;
    public ClientesController(DriveOnContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClienteListDto>>> GetAll([FromQuery] long empresaId, [FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var query = _db.Clientes.AsNoTracking().Where(c => c.EmpresaId == empresaId && c.ExcluidoEm == null);
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(c => c.Nome.ToLower().Contains(q.ToLower()));
        var items = await query.OrderBy(c => c.Nome).Skip((page-1)*pageSize).Take(pageSize)
            .Select(c => new ClienteListDto(c.Id, c.Nome, c.Telefone, c.Email)).ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ClienteDetailDto>> GetById(long id)
    {
        var c = await _db.Clientes.FindAsync(id);
        if (c is null) return NotFound();
        return new ClienteDetailDto(c.Id, c.EmpresaId, c.Nome, c.Telefone, c.Email);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ClienteCreateDto dto)
    {
        var c = new DriveOn.Domain.Entities.Cliente
        {
            EmpresaId = dto.EmpresaId,
            Nome = dto.Nome,
            Telefone = dto.Telefone,
            Email = dto.Email,
            Cpf = dto.Cpf,
            Rua = dto.Rua,
            Numero = dto.Numero,
            Bairro = dto.Bairro,
            Cep = dto.Cep,
            CidadeId = dto.CidadeId,
            Observacoes = dto.Observacoes,
            CriadoEm = DateTimeOffset.UtcNow,
            AtualizadoEm = DateTimeOffset.UtcNow
        };
        _db.Clientes.Add(c);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = c.Id }, new { c.Id });
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, ClienteUpdateDto dto)
    {
        var c = await _db.Clientes.FindAsync(id);
        if (c is null) return NotFound();
        c.Nome = dto.Nome;
        c.Telefone = dto.Telefone;
        c.Email = dto.Email;
        c.Cpf = dto.Cpf;
        c.Rua = dto.Rua;
        c.Numero = dto.Numero;
        c.Bairro = dto.Bairro;
        c.Cep = dto.Cep;
        c.CidadeId = dto.CidadeId;
        c.Observacoes = dto.Observacoes;
        c.AtualizadoEm = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var c = await _db.Clientes.FindAsync(id);
        if (c is null) return NotFound();
        c.ExcluidoEm = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
