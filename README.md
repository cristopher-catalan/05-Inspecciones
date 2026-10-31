# 05 — Sistema de inspecciones

**Estado:** MVP local funcional implementado para práctica; no es una aplicación de producción.  

**Ejecución:** desde la raíz de Preparacion-7-Proyectos-Web-DotNet, ejecuta dotnet run --project .\05-Inspecciones\Inspecciones.csproj y abre http://localhost:5105. Contraseña de demostración: Demo123!. La persistencia usa EF Core InMemory por defecto; los perfiles y roles están en /api/auth/demo-users.

**Objetivo:** planificar inspecciones de sedes/equipos, completar formularios y generar hallazgos trazables.

## Perfiles

- **Coordinador:** crea sedes, plantillas y cronogramas; asigna inspectores.
- **Inspector:** ejecuta una inspección asignada y adjunta evidencias.
- **Supervisor:** revisa hallazgos, devuelve informes y cierra acciones correctivas.
- **Administrador:** gestiona usuarios, catálogos y parámetros.

## Alcance

- Catálogo de establecimientos/equipos.
- Plantillas configurables de inspección, secciones y preguntas.
- Asignación de inspecciones por fecha y responsable.
- Captura de respuestas: conforme, no conforme, no aplica; comentario y evidencia opcional.
- Cálculo de puntaje a partir de reglas explícitas de plantilla.
- Hallazgos con nivel de severidad y acción correctiva.
- Revisión, cierre y exportación de un informe legible.
- Historial de cambios y filtros por sede, responsable, fecha, estado y severidad.

El MVP funciona localmente. El modo offline, la sincronización móvil y la captura de evidencia en campo se reservan para una iteración posterior.

## Datos principales

- **Site:** código, nombre, dirección descriptiva, tipo y activo.
- **InspectionTemplate:** título, versión, estado y vigencia.
- **TemplateSection / Question:** orden, texto, tipo de respuesta, obligatoriedad y puntaje.
- **Inspection:** sede, plantilla/version, inspector, fechas y estado.
- **InspectionAnswer:** respuesta, comentario, puntaje y actor.
- **Finding:** respuesta relacionada, severidad, descripción, responsable y estado correctivo.
- **Evidence:** archivo, ruta, MIME, tamaño y checksum.
- **AuditEvent:** evento, usuario, entidad y fecha.

Una inspección iniciada conserva la versión de plantilla con la que fue creada aunque la plantilla se actualice después.

## Páginas y API indicativa

Páginas: sedes; plantillas; calendario; mis inspecciones; formulario de ejecución; hallazgos; revisión; informe.

Endpoints candidatos: GET/POST /api/sites; GET/POST /api/templates; POST /api/inspections; GET /api/inspections/{id}; PUT /api/inspections/{id}/answers; POST /api/inspections/{id}/submit; POST /api/findings/{id}/close.

## Reglas

- No permitir completar una inspección asignada a otra persona salvo permiso de supervisor.
- Validar preguntas obligatorias antes del envío.
- Mantener respuestas originales y registrar correcciones como eventos.
- Un hallazgo crítico requiere acción correctiva y revisión antes del cierre.
- Restringir y validar archivos de evidencia de la misma forma que en Gestión Documental.
- El cálculo del puntaje debe ser reproducible y mostrar qué respuestas lo generaron.

## Criterios de aceptación

El coordinador crea una inspección desde una plantilla; el inspector completa las preguntas obligatorias; el sistema crea hallazgos para respuestas no conformes según reglas; el supervisor cierra o devuelve el informe; y el historial muestra los responsables y fechas.

## Implementación local actual

MVP disponible: ASP.NET Core + EF Core, panel interactivo y endpoints JSON descritos en el README raíz. El almacenamiento predeterminado es InMemory; SQL Server es configurable. La especificación de este documento sirve como alcance ampliado y backlog.

