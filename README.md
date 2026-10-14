# Labo5 - Leticia Maldonado
## API REST con Clean Architecture (Hexagonal)

Proyecto .NET 10 reorganizado bajo **Clean Architecture** (enfoque Hexagonal: puertos y adaptadores).

---

## Estructura de la solución

```
Labo5-LeticiaMaldonado.sln
│
├── src/
│   ├── Labo5.Domain/                  ← Núcleo puro (sin dependencias externas)
│   │   ├── Entities/                  ← Entidades de negocio (EF Core las mapea, pero no las conoce aquí)
│   │   │   ├── Usuario.cs
│   │   │   ├── Estudiante.cs
│   │   │   ├── Profesore.cs
│   │   │   ├── Curso.cs
│   │   │   ├── Materia.cs
│   │   │   ├── Matricula.cs
│   │   │   ├── Asistencia.cs
│   │   │   └── Evaluacione.cs
│   │   └── Ports/                     ← Interfaces (puertos) de repositorios y UoW
│   │       ├── IGenericRepository.cs
│   │       └── IUnitOfWork.cs
│   │
│   ├── Labo5.Application/             ← Lógica de aplicación (depende solo de Domain)
│   │   ├── DTOs/                      ← Objetos de transferencia de datos (contratos HTTP)
│   │   │   ├── LoginModel.cs
│   │   │   ├── RegisterModel.cs
│   │   │   └── CreateProductDto.cs
│   │   ├── Ports/                     ← Interfaces de servicios externos (puertos)
│   │   │   ├── IJwtService.cs
│   │   │   └── IPasswordHasher.cs
│   │   ├── UseCases/
│   │   │   └── Auth/                  ← Un archivo por caso de uso (principio SRP)
│   │   │       ├── LoginUseCase.cs
│   │   │       └── RegisterUseCase.cs
│   │   └── DependencyInjection.cs     ← AddApplicationServices()
│   │
│   └── Labo5.Infrastructure/          ← Adaptadores (depende de Domain y Application)
│       ├── Data/
│       │   ├── Lab05Context.cs        ← DbContext de EF Core
│       │   └── DbInitializer.cs       ← Semillero de datos iniciales
│       ├── Adapters/
│       │   ├── GenericRepository.cs   ← Implementación de IGenericRepository
│       │   └── UnitOfWork.cs          ← Implementación de IUnitOfWork
│       ├── Services/
│       │   ├── JwtService.cs          ← Implementación de IJwtService (System.IdentityModel)
│       │   └── BCryptPasswordHasher.cs← Implementación de IPasswordHasher (BCrypt.Net)
│       └── DependencyInjection.cs     ← AddInfrastructureServices()
│
└── Labo5-LeticiaMaldonado/            ← Capa de Presentación / API
    ├── Controllers/
    │   └── AuthController.cs          ← Solo llama a Use Cases, nunca a repositorios directos
    ├── ParameterValidationMiddleware.cs
    ├── Program.cs                     ← Configuración de DI, JWT Bearer, Swagger
    └── appsettings.json
```

---

## Explicación de cada capa

### Domain
- **Propósito**: El núcleo de la aplicación. Contiene las entidades del negocio y los contratos (puertos) que definen cómo se accede a los datos.
- **Regla clave**: No depende de ninguna otra capa ni librería externa (ni EF Core, ni BCrypt, ni JWT). Es puro C#.
- **Contiene**: Entidades (`Usuario`, `Curso`, `Estudiante`, etc.) e interfaces `IGenericRepository<T>` e `IUnitOfWork`.

### Application
- **Propósito**: Orquesta los casos de uso del negocio. Cada operación (login, registro) es una clase con un único método público `ExecuteAsync`.
- **Regla clave**: Solo depende de `Domain`. No conoce EF Core ni ninguna librería de infraestructura.
- **Contiene**: Casos de uso (`LoginUseCase`, `RegisterUseCase`), DTOs de entrada/salida e interfaces de servicios externos (`IJwtService`, `IPasswordHasher`).

### Infrastructure
- **Propósito**: Implementa todos los detalles técnicos (adaptadores). Aquí sí se usa EF Core, BCrypt y JWT.
- **Regla clave**: Implementa las interfaces definidas en `Domain` (repositorios, UoW) y en `Application` (JWT, hashing).
- **Contiene**: `Lab05Context`, `GenericRepository`, `UnitOfWork`, `JwtService`, `BCryptPasswordHasher`.

### Api (Presentation)
- **Propósito**: Punto de entrada HTTP. Recibe peticiones, las delega a los casos de uso y devuelve respuestas.
- **Regla clave**: Los controllers solo inyectan y llaman a Use Cases. Nunca acceden a repositorios ni al DbContext directamente.
- **Contiene**: `AuthController`, `ParameterValidationMiddleware`, `Program.cs`.

---

## Flujo de una solicitud

```
HTTP Request
    ↓
ParameterValidationMiddleware
    ↓
AuthController  (Api)
    ↓
LoginUseCase / RegisterUseCase  (Application)
    ↓
IUnitOfWork → IGenericRepository  (Domain Ports)
    ↓
UnitOfWork → GenericRepository  (Infrastructure Adapters)
    ↓
Lab05Context (EF Core)
    ↓
Base de datos MySQL
```

---

## Regla de dependencias

```
Api  ──→  Application  ──→  Domain
           ↑                  ↑
Infrastructure  ─────────────┘
```

**`Domain` no depende de nadie.** Todas las flechas apuntan hacia el núcleo.

---

## Endpoints disponibles

| Método | Ruta | Descripción |
|--------|------|-------------|
| `POST` | `/api/auth/login` | Autenticación, devuelve JWT |
| `POST` | `/api/auth/register` | Registro de nuevo usuario |
| `GET`  | `/api/auth/perfil` | Perfil del usuario autenticado (requiere JWT) |

---

## Stack tecnológico

- **.NET 10** / ASP.NET Core Web API
- **Entity Framework Core** + **Pomelo MySql** (acceso a datos)
- **BCrypt.Net-Next** (hashing de contraseñas)
- **System.IdentityModel.Tokens.Jwt** (generación de tokens)
- **Swashbuckle / Swagger** (documentación de la API)
- **JWT Bearer Authentication**
