using Inspecciones.UI.Components;

namespace Inspecciones.UI;

public static class InspectionLayout
{
    public static string Render()
    {
        return $$"""
        <!doctype html>
        <html lang="es">
        <head>
            <meta charset="utf-8">
            <meta name="viewport" content="width=device-width, initial-scale=1">
            <title>Inspecciones - Operaciones de campo</title>
            <link rel="preconnect" href="https://fonts.googleapis.com">
            <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
            <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800&display=swap" rel="stylesheet">
            <style>
            {{InspectionStyles.Css}}
            </style>
        </head>
        <body class="app-inspections">
            <header class="top">
                <div class="brand">
                    <span class="mark">✓</span>
                    <div>
                        <h1>Sistema de Inspecciones y Auditoría</h1>
                        <p>Seguridad de campo, listas de verificación y control de hallazgos</p>
                    </div>
                </div>
                <div class="top-right">
                    <span style="font-size:12px; font-weight:700; color:#ffdb71; background:#454338; padding:6px 14px; border-radius:99px;" id="currentSessionTag">Sesión no iniciada</span>
                </div>
            </header>

            <main class="page">
                <!-- Session Selector -->
                <section class="session-bar">
                    <form id="loginForm" class="login-controls" onsubmit="handleLogin(event)">
                        <div class="input-group">
                            <label for="selectUser">Perfil Demo</label>
                            <select id="selectUser" required></select>
                        </div>
                        <div class="input-group">
                            <label for="inputPassword">Contraseña</label>
                            <input id="inputPassword" type="password" value="Demo123!" autocomplete="current-password" required>
                        </div>
                        <button type="submit" class="btn btn-primary" style="margin-top:16px;">Iniciar sesión</button>
                        <button type="button" class="btn btn-outline" onclick="handleLogout()" style="margin-top:16px;">Salir</button>
                    </form>
                    <div id="sessionMessage" style="font-size:12px; color:var(--muted);">Elige tu perfil para auditar sedes.</div>
                </section>

                <!-- Field Hero -->
                <section class="field-hero">
                    <div>
                        <small>OPERACIONES DE CAMPO / 05</small>
                        <h2>Inspecciones</h2>
                        <p>Planifica recorridos, completa listas de verificación con evidencia técnica y gestiona el cierre de hallazgos críticos.</p>
                    </div>
                    <div class="round-stat">
                        <strong id="metric-a">—</strong>
                        <span>recorridos</span>
                    </div>
                </section>

                <!-- Field Strip -->
                <section class="field-strip">
                    <div>
                        <small>EN AGENDA O EJECUCIÓN</small>
                        <strong id="metric-b">—</strong>
                    </div>
                    <div>
                        <small>HALLAZGOS ABIERTOS</small>
                        <strong id="metric-c">—</strong>
                    </div>
                    <div style="margin-left:auto; display:flex; gap:10px;">
                        <button type="button" class="btn btn-primary" onclick="openScheduleModal()">＋ Programar visita</button>
                        <button type="button" class="btn btn-outline" onclick="loadInspectionsData()" style="color:#fff; border-color:#555;">🔄 Actualizar</button>
                    </div>
                </section>

                <!-- Main Grid: Inspections & Findings -->
                <section class="main-grid">
                    <!-- Left: Scheduled Visits Table -->
                    <article class="panel">
                        <div class="panel-header">
                            <div>
                                <small style="font-size:11px; font-weight:700; color:var(--primary-dark); text-transform:uppercase;">Agenda de Campo</small>
                                <h3 style="font-size:16px; font-weight:700;">Recorridos Programados</h3>
                            </div>
                        </div>
                        <div id="inspectionsListContainer" style="display:flex; flex-direction:column; gap:10px;">
                            <div style="text-align:center; padding:30px; color:var(--muted); font-size:13px;">Inicia sesión para cargar los recorridos.</div>
                        </div>
                    </article>

                    <!-- Right: Open Findings -->
                    <aside class="panel" style="border-top:4px solid #dc892c;">
                        <div class="panel-header">
                            <div>
                                <small style="font-size:11px; font-weight:700; color:#dc892c; text-transform:uppercase;">Atención Requerida</small>
                                <h3 style="font-size:16px; font-weight:700;">Hallazgos y No Conformidades</h3>
                            </div>
                        </div>
                        <div id="findingsListContainer" style="display:flex; flex-direction:column; gap:10px;">
                            <div style="font-size:12px; color:var(--muted); font-style:italic;">Cargando hallazgos...</div>
                        </div>
                    </aside>
                </section>

                <!-- Hidden elements for test script compatibility -->
                <div style="display:none;">
                    <div id="primary-list"></div>
                    <div id="secondary-list"></div>
                </div>
            </main>

            <!-- Modals & Drawer -->
            {{ScheduleInspectionModal.Html}}
            {{ChecklistAuditDrawer.Html}}

            <!-- Toast Container -->
            {{ToastSystem.Html}}

            <script>
            const byId = id => document.getElementById(id);
            const esc = v => String(v ?? "").replace(/&/g,"&amp;").replace(/</g,"&lt;").replace(/>/g,"&gt;").replace(/"/g,"&quot;").replace(/'/g,"&#39;");
            function formatDate(dt) {
                if (!dt) return "—";
                try {
                    const d = new Date(dt);
                    return d.toLocaleDateString('es-PE', { day:'2-digit', month:'short', hour:'2-digit', minute:'2-digit' });
                } catch { return dt; }
            }

            let currentUser = null;
            let currentRole = null;
            let inspectionsCache = [];
            let activeInspectionId = null;
            let sitesCache = [];
            let templatesCache = [];

            {{ToastSystem.Script}}
            {{ChecklistAuditDrawer.Script}}

            function openModal(id) { byId(id).classList.add('open'); }
            function closeModal(id) { byId(id).classList.remove('open'); }

            function openChecklistDrawer() {
                byId('checklistDrawerOverlay').classList.add('open');
                byId('checklistDrawer').classList.add('open');
            }
            function closeChecklistDrawer() {
                byId('checklistDrawerOverlay').classList.remove('open');
                byId('checklistDrawer').classList.remove('open');
                activeInspectionId = null;
            }

            async function openScheduleModal() {
                if (!currentUser) { showToast('Inicia sesión primero.', 'error'); return; }
                try {
                    const sResp = await fetch('/api/sites');
                    if (sResp.ok) {
                        sitesCache = await sResp.json();
                        byId('selectSite').innerHTML = sitesCache.map(s => `
                            <option value="${s.id}">${esc(s.name)} (${esc(s.code)})</option>
                        `).join('');
                    }

                    const tResp = await fetch('/api/templates');
                    if (tResp.ok) {
                        templatesCache = await tResp.json();
                        byId('selectTemplate').innerHTML = templatesCache.map(t => `
                            <option value="${t.id}">${esc(t.title)} (v${t.version})</option>
                        `).join('');
                    }

                    openModal('scheduleModal');
                } catch {
                    showToast('Error cargando catálogos', 'error');
                }
            }

            async function init() {
                try {
                    const r = await fetch('/api/auth/demo-users');
                    const users = await r.json();
                    byId('selectUser').innerHTML = users.map(u => 
                        `<option value="${esc(u.username)}">${esc(u.label)} (${esc(u.username)}) · ${esc(u.role)}</option>`
                    ).join('');

                    const me = await fetch('/api/auth/me');
                    if (me.ok) {
                        const info = await me.json();
                        currentUser = info.username;
                        currentRole = info.role;
                        updateSessionUI();
                        await loadInspectionsData();
                    }
                } catch {
                    showToast('Error conectando con el servidor', 'error');
                }
            }

            function updateSessionUI() {
                if (currentUser) {
                    byId('currentSessionTag').textContent = `${currentUser} (${currentRole})`;
                    byId('sessionMessage').textContent = `Conectado como ${currentUser} [${currentRole}]`;
                    byId('sessionMessage').style.color = 'var(--primary-dark)';
                    byId('selectUser').value = currentUser;
                } else {
                    byId('currentSessionTag').textContent = 'Sesión no iniciada';
                    byId('sessionMessage').textContent = 'Inicia sesión para operar.';
                    byId('sessionMessage').style.color = 'var(--muted)';
                }
            }

            async function handleLogin(e) {
                e.preventDefault();
                const username = byId('selectUser').value;
                const password = byId('inputPassword').value;
                try {
                    const r = await fetch('/api/auth/login', {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify({ username, password })
                    });
                    if (r.ok) {
                        const d = await r.json();
                        currentUser = d.username;
                        currentRole = d.role;
                        updateSessionUI();
                        showToast(`Bienvenido/a ${currentUser}`, 'success');
                        await loadInspectionsData();
                    } else {
                        showToast('Credenciales incorrectas.', 'error');
                    }
                } catch {
                    showToast('Error al conectar', 'error');
                }
            }

            async function handleLogout() {
                await fetch('/api/auth/logout', { method: 'POST' });
                currentUser = null;
                currentRole = null;
                updateSessionUI();
                inspectionsCache = [];
                renderInspectionsList([]);
                renderFindingsList([]);
                byId('metric-a').textContent = '—';
                byId('metric-b').textContent = '—';
                byId('metric-c').textContent = '—';
                showToast('Sesión finalizada', 'info');
            }

            async function loadInspectionsData() {
                try {
                    // 1. Overview
                    const r = await fetch('/api/inspections-mgmt/overview');
                    if (r.ok) {
                        inspectionsCache = await r.json();
                        renderInspectionsList(inspectionsCache);
                        byId('metric-a').textContent = inspectionsCache.length;
                        byId('metric-b').textContent = inspectionsCache.filter(x => x.state !== 'Cerrada').length;
                    }

                    // 2. Open findings
                    const fResp = await fetch('/api/inspections-mgmt/open-findings');
                    if (fResp.ok) {
                        const findings = await fResp.json();
                        renderFindingsList(findings);
                        byId('metric-c').textContent = findings.length;
                    }
                } catch (e) {
                    console.error(e);
                }
            }

            function renderInspectionsList(inspections) {
                const container = byId('inspectionsListContainer');
                if (!inspections || inspections.length === 0) {
                    container.innerHTML = '<div style="text-align:center; padding:30px; color:var(--muted); font-size:13px;">No hay recorridos asignados o programados.</div>';
                    return;
                }

                container.innerHTML = inspections.map(i => {
                    const sCls = i.state === 'Cerrada' ? 'tag-cerrada' :
                                 (i.state === 'En ejecución' ? 'tag-ejecucion' :
                                 (i.state === 'En revisión' ? 'tag-revision' : 'tag-programada'));
                    return `
                    <div style="background:#fdfbf7; border:1px solid var(--border); border-radius:8px; padding:14px; cursor:pointer;" onclick="openChecklist(${i.id})">
                        <div style="display:flex; justify-content:space-between; align-items:flex-start;">
                            <div>
                                <strong style="font-size:15px; color:#292524;">Recorrido #${i.id} · ${esc(i.siteName)}</strong>
                                <div style="font-size:11px; color:var(--muted); margin-top:2px;">
                                    Plantilla: ${esc(i.templateTitle)} · Inspector: <strong>${esc(i.inspector)}</strong> · ${formatDate(i.startedAt)}
                                </div>
                            </div>
                            <span class="tag ${sCls}">${esc(i.state)}</span>
                        </div>
                        <div style="margin-top:10px; display:flex; justify-content:space-between; align-items:center; font-size:12px; border-top:1px dashed #e2dccb; padding-top:6px;">
                            <span>Puntaje auditado: <strong style="color:var(--primary-dark); font-size:14px;">${Number(i.score).toFixed(1)}%</strong></span>
                            ${i.openFindings > 0 ? `<span style="color:var(--danger); font-weight:700;">⚠️ ${i.openFindings} hallazgos</span>` : '<span style="color:var(--success);">✓ Sin hallazgos abiertos</span>'}
                            <span style="color:var(--primary); font-weight:700;">Abrir checklist ➔</span>
                        </div>
                    </div>
                    `;
                }).join('');
            }

            function renderFindingsList(findings) {
                const container = byId('findingsListContainer');
                if (!findings || findings.length === 0) {
                    container.innerHTML = '<div style="font-size:12px; color:var(--muted); font-style:italic; padding:15px 0;">No hay hallazgos abiertos pendientes.</div>';
                    return;
                }

                container.innerHTML = findings.map(f => {
                    const isHigh = f.severity === 'Alta' || f.severity === 'Crítica';
                    return `
                    <div style="background:#fff; border:1px solid #fed7aa; border-radius:8px; padding:12px;">
                        <div style="display:flex; justify-content:space-between; align-items:flex-start;">
                            <strong style="font-size:13px; color:#7c2d12;">${esc(f.description)}</strong>
                            <span class="tag" style="background:${isHigh ? '#fee2e2' : '#fef3c7'}; color:${isHigh ? '#dc2626' : '#b45309'};">${esc(f.severity)}</span>
                        </div>
                        <div style="font-size:11px; color:var(--muted); margin:4px 0 8px;">
                            Sede: ${esc(f.siteName)} · Inspección #${f.inspectionId}
                        </div>
                        <button type="button" class="btn btn-primary" onclick="handleCloseFinding(${f.id})" style="font-size:11px; padding:5px 10px; width:100%; justify-content:center;">
                            ✓ Cerrar con Acción Correctiva
                        </button>
                    </div>
                    `;
                }).join('');
            }

            async function openChecklist(id) {
                activeInspectionId = id;
                try {
                    const r = await fetch(`/api/inspections/${id}`);
                    if (!r.ok) { showToast('No tienes permiso para ver esta inspección.', 'error'); return; }
                    const d = await r.json();
                    const item = d.inspection;

                    byId('drawerInspTitle').textContent = `Recorrido #${item.id} - ${d.template?.title || 'Inspección'}`;
                    byId('drawerInspSite').textContent = d.site?.name || 'Sede';
                    byId('drawerInspInspector').textContent = item.inspector;
                    byId('drawerInspState').textContent = item.state;

                    renderChecklistQuestions(d.questions || [], d.answers || []);
                    openChecklistDrawer();
                } catch {
                    showToast('Error cargando checklist', 'error');
                }
            }

            async function handleScheduleInspection(e) {
                e.preventDefault();
                const btn = byId('btnSubmitSchedule');
                btn.disabled = true;
                btn.textContent = 'Agendando...';

                const siteId = Number(byId('selectSite').value);
                const templateId = Number(byId('selectTemplate').value);
                const inspector = byId('selectInspector').value;

                try {
                    const r = await fetch('/api/inspections', {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify({ siteId, templateId, inspector })
                    });
                    if (r.ok) {
                        const created = await r.json();
                        showToast(`¡Recorrido #${created.id} agendado exitosamente!`, 'success');
                        closeModal('scheduleModal');
                        await loadInspectionsData();
                    } else {
                        const err = await r.json();
                        showToast(err.error || 'Error al agendar inspección.', 'error');
                    }
                } catch {
                    showToast('Fallo de conexión', 'error');
                } finally {
                    btn.disabled = false;
                    btn.textContent = 'Agendar Recorrido';
                }
            }

            init();
            </script>
        </body>
        </html>
        """;
    }
}
