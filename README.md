## MarketPlace API - .NET 8

Proyecto orientado a demostrar un diseño de software empresarial en .NET 8, aplicando Domain-Driven Design (DDD), separación estricta de responsabilidades y foco en la protección de las reglas de negocio.

El dominio modela un marketplace, con especial atención a la consistencia de los agregados, la imposibilidad de estados inválidos y una arquitectura pensada para evolucionar, escalar e integrarse con sistemas externos.

Este repositorio refleja cómo abordo proyectos reales en entornos profesionales, tanto desde el punto de vista técnico como organizativo.

## Objetivos del proyecto

- Modelar un dominio rico con reglas de negocio protegidas por diseño
- Evitar estados inválidos incluso ante un uso incorrecto de las capas superiores
- Mantener una arquitectura mantenible, testeable y extensible
- Tratar frameworks y persistencia como detalles de implementación
- Emular un desarrollo evolutivo, similar a un producto real en producción

## Arquitectura

El proyecto sigue los principios de Clean Architecture, con separación clara entre capas:

**Domain**
Contiene el modelo de negocio, reglas e invariantes. No depende de ninguna otra capa.

**Application**
Define los casos de uso, contratos y orquestación de la lógica de negocio.

**Infrastructure**
Implementa persistencia y dependencias externas (EF Core, base de datos, etc.).

**API**
Capa de exposición (en desarrollo), responsable únicamente de entrada/salida.

## Dominio y reglas de negocio
El dominio está diseñado para proteger las invariantes de negocio por diseño, no solo mediante validaciones superficiales.

# Principios aplicados
1. Entidades con constructores privados o internal
2. Creación exclusiva mediante métodos factory estáticos
3. Sin setters públicos
4. Todas las modificaciones se realizan mediante métodos del dominio
5. Uso de Result<T> para representar explícitamente éxito o error
6. El dominio no expone estados inválidos posibles

# Agregados
Ejemplo clave: Order como Aggregate Root
1. OrderItem y Payment no pueden existir fuera de una Order
2. Al crear una Order:
  - Se crean automáticamente los OrderItems
  - Se crea el Payment
3. No existe ninguna forma válida de:
  - Crear un Payment aislado
  - Crear un OrderItem fuera de una Order
4. Estas restricciones están:
  - Reforzadas por el diseño
  - Cubiertas por tests unitarios
  - Imposibles de romper desde capas superiores

## CQRS y capa de aplicación
La capa de aplicación implementa los casos de uso del sistema mediante:
1. CQRS
  - Commands / Queries separados
  - Handlers independientes
2. Interfaces de repositorios y Unit of Work
3. Servicios de aplicación cuando la lógica:
  - Es extensa
  - No pertenece al dominio puro
4. Orquestación clara sin duplicar reglas de negocio

La aplicación no contiene lógica de persistencia ni dependencias técnicas.

## Testing y calidad
El proyecto prioriza la verificación del comportamiento, no solo la cobertura.
# Tests unitarios
**Dominio**
- Reglas de creación
- Restricciones
- Comportamiento ante estados inválidos.

**Aplicación**
- Casos de uso
- Flujo correcto de operaciones
# Tests de integración
- Persistencia real con Entity framework core
- Validación de:
  - Configuración de DbContext.
  - Funcionamiento de repositorios concretos.
  - UnitOfWork como punto único de guardado.
• Creación y eliminación de una única **base de datos auxiliar aislada** del sistema para la realización de todas las pruebas.

## Infraestructura y persistencia
- Entity Framework Core como detalle de persistencia
- DbContext aislado en la capa de infraestructura
- Repositorios concretos heredando de un repositorio base con CRUD común
- UnitOfWork para coordinar transacciones
- ServiceCollectionExtensions para:
  - Registrar dependencias
  - Mantener limpia la configuración
La infraestructura implementa contratos, no define reglas.

## Estado actual del proyecto
a arquitectura base y el dominio están **completamente definidos y funcionales**.

Algunas partes se encuentran en desarrollo activo como parte de una **estrategia evolutiva**, similar a un producto real.

# Roadmap

- ✔️ Diseño y construcción del dominio (DDD)
- ✔️ Infraestructura y persistencia
- ✔️ Base de la capa de aplicación (CQRS, handlers, Result pattern)
- ✔️ Finalización de la capa de aplicación
- 🔄 Desarrollo de una API de autenticación
- 🔄 Integración de autenticación y autorización en el marketplace
- 🔄 Endpoints y eventos.
- 🔄 Tests de regresión con Postman

