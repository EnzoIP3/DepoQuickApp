# HomeConnect

![Build and Test - Develop](https://github.com/IngSoft-DA2/283145_294238_285727/actions/workflows/build-and-test.yml/badge.svg?branch=develop&event=push)
![Build and Test - Main](https://github.com/IngSoft-DA2/283145_294238_285727/actions/workflows/build-and-test.yml/badge.svg?branch=main&event=push)
![Clean Code - Develop](https://github.com/IngSoft-DA2/283145_294238_285727/actions/workflows/build-and-test.yml/badge.svg?branch=develop&event=push)
![Clean Code - Main](https://github.com/IngSoft-DA2/283145_294238_285727/actions/workflows/build-and-test.yml/badge.svg?branch=main&event=push)

A smart home platform developed as part of a university project, **HomeConnect** allows users with different roles to manage homes, rooms, smart devices, notifications, and permissions. Designed with clean architecture principles, this system is modular, extensible, and highly testable.

👉 [Versión en Español](#versión-en-español)

---

## 🧩 Architecture Overview

- **BusinessLogic**: Domain entities and business logic services/interfaces.
- **DataAccess**: Repository implementations using Entity Framework Core.
- **WebApi**: ASP.NET Core REST API exposing all business operations.
- **Frontend**: Angular SPA built with PrimeNG and PrimeFlex.

## ⚙️ Tech Stack

- .NET 7 + ASP.NET Core
- Angular + PrimeNG
- SQL Server (via Docker)
- Entity Framework Core
- Clean Architecture
- Custom Auth (Guid token system)
- Dynamic plugins via reflection (DLL-based importers/validators)
- 97% code coverage

---

## 🚀 Local Development Setup

### 🐳 1. Run SQL Server in Docker

From the root folder:

```bash
cd Aplicacion
docker compose up -d
```

Make sure to update the `SA_PASSWORD` and `ports` if needed in `docker-compose.yml`. Note: It is not recommended to use sensitive credentials (e.g., passwords) directly in docker-compose.yml in production environments, as it could expose your credentials. However, this approach was used for the project setup and was not the main focus of the project.

### 🛠 2. Configure Connection Strings

In `Aplicacion/appsettings.json`, under `"Production"`, update your connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=HomeConnect;User Id=sa;Password=Passw1rd;"
}
```

### 🧪 3. Restore Database (Optional)

Use the provided `.bak` files to restore an empty or preloaded database with test data.

### 🧬 4. Run WebApi

```bash
cd Aplicacion/WebApi
dotnet run
```

### 🌐 5. Run Angular Frontend

```bash
cd Aplicacion/frontend
npm install
ng serve
```

Edit the `apiUrl` in `frontend/src/environments/environment.ts` if you're using a different port for the API.

---

## 🔌 Extensibility Highlights

- **New device types** via inheritance and `DeviceFactory`
- **External device importers** via plugin architecture (`Importers/`)
- **Custom model validators** from third-party DLLs (`Validators/`)
- **Polymorphic controller logic** for devices (e.g., lamps, sensors)

---

## 🧑‍💻 Authors

Project developed for **Diseño de Aplicaciones 2** — Universidad ORT Uruguay

- Enzo Izquierdo  
- Manuel Graña  
- Martín Salaberry

---

# Versión en Español

## 📘 Descripción

**HomeConnect** es una plataforma web para gestionar hogares, dispositivos inteligentes, miembros y notificaciones. Permite la administración centralizada de dispositivos mediante una arquitectura limpia, modular y altamente extensible.

---

## 🚀 Instrucciones para entorno de desarrollo

### 🐳 1. Ejecutar SQL Server con Docker

Desde la carpeta raíz:

```bash
cd Aplicacion
docker compose up -d
```

Podés modificar la contraseña de `SA_PASSWORD` y el puerto en `docker-compose.yml`. Nota: No se recomienda usar credenciales sensibles (como contraseñas) directamente en docker-compose.yml en entornos de producción, ya que puede exponer tus credenciales. Sin embargo, este enfoque se utilizó para la configuración del proyecto y no era el enfoque principal del proyecto.

### 🛠 2. Configurar cadenas de conexión

En `Aplicacion/appsettings.json`, dentro de `"Production"`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=HomeConnect;User Id=sa;Password=Passw1rd;"
}
```

### 🧪 3. Restaurar base de datos (opcional)

Se incluyen backups `.bak` con datos de prueba o estructura vacía.

### 🧬 4. Ejecutar WebApi

```bash
cd Aplicacion/WebApi
dotnet run
```

### 🌐 5. Ejecutar Frontend Angular

```bash
cd Aplicacion/frontend
npm install
ng serve
```

Verificá el archivo `environment.ts` para confirmar el `apiUrl`.

---

## 💡 Características destacadas

- Arquitectura basada en capas (BusinessLogic, DataAccess, WebApi, Frontend)
- Plugins externos (importadores y validadores)
- Manejo avanzado de permisos y roles
- Paginación con patrón Template Method
- Controladores polimórficos reutilizables
- Cobertura de pruebas del 97%

---

Proyecto desarrollado por:

- Enzo Izquierdo  
- Manuel Graña  
- Martín Salaberry

Materia: **Diseño de Aplicaciones 2** – Universidad ORT Uruguay
```
