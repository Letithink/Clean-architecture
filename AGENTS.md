# Instrucciones para el agente: migrar este proyecto .NET a Clean Architecture (Hexagonal)

## Objetivo
Reorganizar el proyecto .NET existente en Clean Architecture (enfoque Hexagonal: puertos y adaptadores), sin romper la funcionalidad actual. Es una tarea de reorganización, no de reescritura: mueve y adapta el código existente, no inventes funcionalidad nueva.

---

## Reglas generales
1. **Antes de tocar nada**: analiza la solución actual (proyectos, carpetas, entidades, repositorios, servicios, controllers, Program.cs) y muéstrame un plan con el mapa "archivo actual -> destino". Espera mi confirmación.
2. **Haz el trabajo por pasos pequeños y compila (`dotnet build`) después de cada paso**. Si algo no compila, corrígelo antes de seguir.
3. **No cambies nombres de endpoints, rutas ni contratos de la API** (DTOs de entrada/salida) salvo que sea imprescindible, y avísame si lo es.
4. **No elimines código sin confirmar**. Si un archivo ya no tiene lugar, dime cuál es y por qué.
5. **Respeta el stack**: .NET, EF Core (DbContext), LINQ, Swagger, JWT. No introduzcas Spring ni otros frameworks.
6. **Lenguaje del código**: mantén el idioma y estilo de nombres que ya usa el proyecto.

---

## Estructura de capas objetivo

Tres capas + la capa de entrada (API):

```text
/Domain            -> núcleo: entidades y puertos del dominio
/Application       -> casos de uso, DTOs, interfaces (puertos) de servicios externos
/Infrastructure    -> adaptadores: EF Core, repositorios, Unit of Work, servicios externos
/Api (Presentation)-> controllers, middlewares, Program.cs, configuración de Swagger
```

### Domain
- Entidades (`models`) y sus reglas básicas.
- Interfaces de repositorios (`IRepository<T>` genérico, `IProductRepository`, etc.) e `IUnitOfWork`. Estas interfaces son los puertos.
- No depende de nada: ni de EF Core, ni de librerías externas, ni de las otras capas.

### Application
- Casos de uso (*Use Cases*): reemplazan al patrón Servicios para la lógica de negocio.
- DTOs, mapeos y validaciones de entrada.
- Interfaces de servicios externos (ej. `ITokenService`, `IFileStorageService`) como puertos.
- Depende solo de `Domain`.

### Infrastructure
- `DbContext`, configuración de entidades, migraciones, cadena de conexión.
- Implementaciones de repositorios y de Unit of Work (adaptadores).
- Implementaciones de servicios externos (JWT, Azure, AWS S3, etc.).
- Depende de `Application` y `Domain`.

### Api (Presentation)
- Controllers, middlewares, configuración de Swagger, CORS, autenticación y registro de dependencias en `Program.cs`.
- El controller solo llama a casos de uso, nunca directo a repositorios ni al `DbContext`.
- Depende de `Application` e `Infrastructure` (esta última solo para la inyección de dependencias).

> **Regla de dependencias:** Las flechas apuntan siempre hacia el núcleo (`Api` -> `Application` -> `Domain`; `Infrastructure` -> `Application` / `Domain`). `Domain` no conoce a nadie.

---

## Flujo de una solicitud (debe quedar así)
`Request -> (Middlewares) -> Controller -> Use Case -> Unit of Work / Repositorio -> DbContext (EF Core) -> BD`

*Las consultas se arman con LINQ.*

---

## Casos de uso (lo más importante)
- **Antes:** Un `ProductService` con todos los métodos (`GetAll`, `GetById`, `Insert`, `Update`, `Delete` + específicos).
- **Ahora:** Un archivo (una clase) por cada método, con responsabilidad única (principio S de SOLID).
- **Organización:** Una carpeta por entidad dentro de `Application/UseCases/`, y dentro un archivo por caso de uso.

```text
Application/UseCases/Products/
    GetAllProductsUseCase.cs
    GetProductByIdUseCase.cs
    InsertProductUseCase.cs
    UpdateProductUseCase.cs
    DeleteProductUseCase.cs
    GetHighestPricedProductUseCase.cs   (los métodos específicos también van aparte)
```

- Los casos de uso no usan interfaz (se inyectan como clase concreta). Registrarlos en DI.
- Cada caso de uso expone un único método público (por ejemplo `ExecuteAsync`).
- Un caso de uso llama al Unit of Work / repositorio, nunca al `DbContext` directamente.
- Es el único componente al que se le tolera tener más líneas de código.

---

## Repositorios y Unit of Work
- Un repositorio por entidad, usando un repositorio genérico solo para los métodos repetitivos (`GetAll`, `GetById`, `Add`, `Update`, `Delete`).
- Los métodos específicos de una entidad van en su repositorio propio.
- El Unit of Work es el concentrador de repositorios; mantenlo y colócalo: interfaz en `Domain`, implementación en `Infrastructure`.

---

## Servicios (nuevo rol)
Los "servicios" ya **NO** contienen lógica de negocio. Solo se usan para lógica que depende de librerías o sistemas externos:
- Generación de tokens/claims JWT.
- Subida/descarga de archivos (Azure Blob, AWS S3).
- Cualquier integración externa.

> Interfaz en `Application` (puerto), implementación en `Infrastructure` (adaptador). Los casos de uso pueden llamarlos.

---

## Pasos de migración sugeridos
1. Analizar la solución y proponer el plan (esperar confirmación).
2. Crear los proyectos/carpetas de capas y las referencias entre ellos respetando la regla de dependencias.
3. Mover entidades e interfaces de repositorios/UoW a `Domain`.
4. Mover `DbContext`, repositorios, UoW y migraciones a `Infrastructure`.
5. Convertir cada método de cada servicio en un caso de uso en `Application` (carpeta por entidad). Los servicios que dependen de librerías externas se quedan como servicios (interfaz + implementación).
6. Mover DTOs y validaciones a `Application`.
7. Dejar en `Api` solo controllers, middlewares, Swagger y `Program.cs`; actualizar los controllers para llamar a casos de uso.
8. Registrar todo en inyección de dependencias (puede ser un método de extensión por capa: `AddApplication()`, `AddInfrastructure()`).
9. `dotnet build` y `dotnet run`; verificar en Swagger que los endpoints siguen funcionando.
10. Resumen final: qué se movió, qué se creó, qué quedó pendiente.

---

## Qué NO hacer
- No poner lógica de negocio en controllers ni en repositorios.
- No referenciar EF Core ni librerías externas desde `Domain`.
- No dejar un "servicio gigante" por entidad.
- No hacer un CRUD simplón nuevo: el objetivo es reorganizar lo existente.
- No usar la estructura de la sesión 8 (laboratorio de LINQ avanzado); esta tarea es sobre el proyecto base.

---

## Entrega
- El proyecto reorganizado se sube a GitHub (se entrega el link del repositorio).
- Crea o actualiza un `README.md` breve con: estructura de carpetas, explicación de cada capa y el flujo de la solicitud.
