# Service.Empresa

Microservicio de gestión de empresas, configuraciones tributarias y parámetros contables para la plataforma **Monocont**.

---

## 📌 Características
- Registro, actualización y consulta de empresas por RUC.
- Validación de RUC (algoritmo Módulo 11 de SUNAT).
- Gestión de régimen tributario (MYPE Tributario, Régimen General, RER, Especial).
- Control de periodos contables y vinculación de usuarios autorizados.
- Arquitectura Limpia (**Clean Architecture**) con CQRS y **MediatR**.
- Seguridad y autorización validada por JWT emitido por `Service.Seguridad`.
- Persistencia en **PostgreSQL** con **Entity Framework Core**.

---

## 🏗 Arquitectura y Estructura
```
Service.Empresa/
├── Service.Empresa.API/            # Controladores REST, Endpoints y Program.cs
├── Service.Empresa.Application/    # Commands, Queries, DTOs, Validadores e Interfaces
├── Service.Empresa.Domain/         # Entidades de Empresa, Configuración y Regímenes
└── Service.Empresa.Infrastructure/ # DbContext, Repositorios y Scripts SQL
```

---

## ⚙️ Configuración y Variables de Entorno

Archivo: `appsettings.json`

```json
{
  "ConnectionStrings": {
    "EmpresaDb": "Host=localhost;Port=5432;Database=monocont_empresa;Username=postgres;Password=postgres"
  },
  "Cors": {
    "AllowedOrigins": [
      "http://localhost:4200",
      "https://localhost:4200"
    ]
  }
}
```

---

## 🚀 Ejecución

### Puerto por defecto: `5001`

```bash
cd Service.Empresa.API
dotnet run
```
- **Swagger UI:** `http://localhost:5001/swagger`

---

## 🗄 Base de Datos
Ejecutar los scripts SQL ubicados en `Service.Empresa.Infrastructure/Script/` en la base de datos `monocont_empresa`.
