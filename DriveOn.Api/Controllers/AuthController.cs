using DriveOn.Application.Auth;
using DriveOn.Infrastructure.Persistence;
using DriveOn.Infrastructure.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DriveOn.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly DriveOnContext _db;
    private readonly IPasswordHasher _hasher;
    private readonly IConfiguration _config;

    public AuthController(DriveOnContext db, IPasswordHasher hasher, IConfiguration config)
    {
        _db = db;
        _hasher = hasher;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] DriveOn.Application.Usuarios.UsuarioCreateDto dto)
    {
        // valida FK antes: empresa precisa existir
        var empresaExists = await _db.Empresas.AnyAsync(e => e.Id == dto.EmpresaId);
        if (!empresaExists) return BadRequest("Empresa inexistente.");

        var userExists = await _db.Usuarios.AnyAsync(u => u.EmpresaId == dto.EmpresaId && u.Email == dto.Email);
        if (userExists) return Conflict("E-mail já utilizado na empresa.");

        var usuario = new DriveOn.Domain.Entities.Usuario
        {
            EmpresaId = dto.EmpresaId,
            Nome = dto.Nome,
            Email = dto.Email,
            Cargo = dto.Cargo,
            SenhaHash = _hasher.Hash(dto.Password),
            SenhaAtualizadaEm = DateTimeOffset.UtcNow,
            CriadoEm = DateTimeOffset.UtcNow,
            AtualizadoEm = DateTimeOffset.UtcNow,
            Ativo = true
        };

        _db.Usuarios.Add(usuario);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Login), new { id = usuario.Id }, new { usuario.Id, usuario.Nome, usuario.Email });
    }

    [HttpPost("login-email")]
    public async Task<ActionResult<LoginResponse>> LoginByEmail([FromBody] LoginByEmailRequest req)
    {
        var email = (req.Email ?? "").Trim();
        var password = (req.Password ?? "").Trim();

        // busca por e-mail normalizado (CITEXT já é case-insensitive, mas o Trim evita espaços)
        var users = await _db.Usuarios
            .AsNoTracking()
            .Where(u => u.Ativo && u.Email == email)
            .ToListAsync();

        if (users.Count == 0)
            return Unauthorized("E-mail ou senha inválidos.");

        if (users.Count > 1)
        {
            var empresaIds = users.Select(u => u.EmpresaId).Distinct().ToList();
            var empresas = await _db.Empresas
                .Where(e => empresaIds.Contains(e.Id))
                .Select(e => new { e.Id, e.Nome })
                .ToListAsync();

            return Conflict(new {
                code = "MULTIPLE_COMPANIES",
                message = "Selecione a empresa para continuar.",
                empresas
            });
        }

        var user = users[0];
        if (!_hasher.Verify(password, user.SenhaHash))
            return Unauthorized("E-mail ou senha inválidos.");

        return CreateJwt(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest req)
    {
        var email = (req.Email ?? "").Trim();
        var password = (req.Password ?? "").Trim();

        var user = await _db.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Ativo && u.EmpresaId == req.EmpresaId && u.Email == email);

        if (user is null || !_hasher.Verify(password, user.SenhaHash))
            return Unauthorized("E-mail ou senha inválidos.");

        return CreateJwt(user);
    }

    private ActionResult<LoginResponse> CreateJwt(DriveOn.Domain.Entities.Usuario user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim("empresaId", user.EmpresaId.ToString()),
            new Claim(ClaimTypes.Role, user.Cargo),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return new LoginResponse(jwt, user.Id, user.Nome, user.Cargo, user.EmpresaId);
    }
}
