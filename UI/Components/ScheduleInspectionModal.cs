namespace Inspecciones.UI.Components;

public static class ScheduleInspectionModal
{
    public const string Html = """
    <div id="scheduleModal" class="modal-overlay">
        <div class="modal">
            <div class="modal-header">
                <h3>Programar Recorrido de Inspección</h3>
                <button type="button" class="btn btn-outline" onclick="closeModal('scheduleModal')">✕</button>
            </div>
            <form id="scheduleForm" onsubmit="handleScheduleInspection(event)">
                <div class="modal-body">
                    <div class="form-group">
                        <label for="selectSite">Sede o Establecimiento *</label>
                        <select id="selectSite" name="siteId" required></select>
                    </div>
                    <div class="form-group">
                        <label for="selectTemplate">Plantilla de Verificación *</label>
                        <select id="selectTemplate" name="templateId" required></select>
                    </div>
                    <div class="form-group">
                        <label for="selectInspector">Inspector Asignado *</label>
                        <select id="selectInspector" name="inspector" required>
                            <option value="inspector">Inspector Principal (inspector)</option>
                            <option value="supervisor">Supervisor de Campo (supervisor)</option>
                            <option value="admin">Administrador (admin)</option>
                        </select>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline" onclick="closeModal('scheduleModal')">Cancelar</button>
                    <button type="submit" class="btn btn-primary" id="btnSubmitSchedule">Agendar Recorrido</button>
                </div>
            </form>
        </div>
    </div>
    """;
}
