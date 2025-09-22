# DriveOn Backend (ASP.NET Core 8 + PostgreSQL)

Camadas:
- **DriveOn.Domain**: Entidades do domínio
- **DriveOn.Application**: DTOs e contratos
- **DriveOn.Infrastructure**: EF Core, Contexto, Hash de senha
- **DriveOn.Api**: Web API (REST + MVC Controllers)

## Rodando com Docker
```bash
docker compose up --build
```
A API sobe em `http://localhost:5080/swagger`.

## Migrações
No primeiro run, `Database.Migrate()` aplica/gera as tabelas a partir do modelo.
Para criar migrações localmente:
```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add Initial --project DriveOn.Api --startup-project DriveOn.Api
dotnet ef database update --project DriveOn.Api --startup-project DriveOn.Api
```

## Endpoints principais
- `POST /api/auth/register` – cria usuário (hash Bcrypt)
- `POST /api/auth/login` – retorna JWT
- CRUDs:
  - `/api/empresas`
  - `/api/clientes`
  - `/api/veiculos`
  - `/api/itens`
- OS:
  - `POST /api/os` – cria OS com itens e calcula total
  - `GET /api/os/{id}` – detalhes
  - `PUT /api/os/{id}` – atualiza status/descrição/desconto
- Financeiro:
  - `POST /api/financeiro/contas-receber`
  - `POST /api/financeiro/contas-pagar`
  - `POST /api/financeiro/movimentos` – atualiza status automático (aberto/parcial/pago)

## Observações
- Schema padrão `driveon`.
- Soft delete aplicado em entidades com `ExcluidoEm`.
- Ajuste a `Jwt:Key` em `appsettings.json`.
- Adicione validações/AutoMapper/Profiles conforme necessidade.
