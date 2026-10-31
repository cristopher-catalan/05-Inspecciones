# 05 — Sistema de inspecciones y seguridad de campo

**Estado:** Aplicación web interactiva para auditorías y recorridos de campo con cuestionario digital dinámico, cálculo en vivo de puntaje de conformidad y flujo de cierre de hallazgos.

**Ejecución:** desde la raíz de Preparacion-7-Proyectos-Web-DotNet, ejecuta `dotnet run --project .\05-Inspecciones\Inspecciones.csproj` y abre `http://localhost:5105`. Contraseña de demostración: `Demo123!`.

---

## Características de la Interfaz Moderna y Flujos Dinámicos

1. **Agenda de Recorridos en Tiempo Real:**
   - Programación de inspecciones asignando sede, plantilla normativa e inspector responsable.
   - Aislamiento por inspector: los perfiles de inspector solo visualizan sus recorridos asignados.

2. **Cuestionario Digital de Auditoría (*Checklist*):**
   - Panel lateral interactivo con preguntas estructuradas y ponderadas.
   - Botones rápidos de 1 clic: **"✓ Conforme"**, **"✗ No conforme"** y **"— No aplica"**.
   - Campo para ingresar observaciones y comentarios por ítem.

3. **Cálculo y Barra de Puntaje en Vivo:**
   - La barra de conformidad se actualiza en tiempo real al seleccionar respuestas, cambiando de color dinámicamente según el porcentaje alcanzado (verde $\ge 80\%$, ámbar $\ge 50\%$, rojo $< 50\%$).

4. **Generación Automática de Hallazgos y Cierre con Acción Correctiva:**
   - Cualquier respuesta marcada como "No conforme" genera automáticamente un hallazgo con severidad (*Alta*, *Media*, *Baja*).
   - Botón directo de **1 clic para supervisores: "✓ Cerrar con Acción Correctiva"** exigiendo el registro de la medida técnica implementada.

---

## Arquitectura de Código

```
05-Inspecciones/
├── Domain/
│   └── InspectionModels.cs          # DTOs de recorridos, checklist y hallazgos
├── Services/
│   └── InspectionScoringService.cs  # Ponderación de puntajes y cálculo de severidad
├── Endpoints/
│   └── InspectionEndpoints.cs       # Endpoints de recorridos y hallazgos abiertos
├── UI/
│   ├── InspectionStyles.cs          # Paleta industrial de seguridad y medidores
│   ├── InspectionLayout.cs          # Layout con round-stat y panel de hallazgos
│   └── Components/
│       ├── ScheduleInspectionModal.cs # Modal para agendar visitas
│       ├── ChecklistAuditDrawer.cs    # Cuestionario interactivo y barra de score
│       └── ToastSystem.cs             # Notificaciones flotantes animadas
└── Program.cs                       # Pipeline y servicios
```

---

## Verificación Funcional

Compilación limpia en .NET 10. Valida cálculo de score al 50%, generación de hallazgo y cierre supervisado en `Review-FunctionalFlows.ps1`.
